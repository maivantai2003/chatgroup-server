
using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Hubs;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.RabbitMQ.Interfaces;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace chatgroup_server.RabbitMQ.Consumer
{
    public class BirthDayConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IChannel? _channel;
        private readonly string ExchangeName = "birthday.exchange";
        private readonly string QueueName = "birthday.queue";
        private readonly IRabbitMQConnection _connection;
        private readonly ILogger<BirthDayConsumer> _logger;
        public BirthDayConsumer(IServiceProvider serviceProvider,IRabbitMQConnection connection, ILogger<BirthDayConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _connection = connection;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var conn = await _connection.GetConnectionAsync();
            _channel = await conn.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Fanout, durable: true);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(QueueName, ExchangeName, routingKey: "");
            await _channel.BasicQosAsync(0, 3, false);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IConversationService>();
                var hub = scope.ServiceProvider.GetRequiredService<IHubContext<myHub>>();

                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var data = JsonConvert.DeserializeObject<BirthdayModel>(body);
                    Console.WriteLine("json: " + data);
                    if (data == null)
                    {
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                        return;
                    }
                    var conversation = new Conversation()
                    {
                        UserId = data.SenderId,
                        Id = data.FriendId,
                        Type = "user",
                        UserSend = data.UserName,
                        Content = $"🎉 Hôm nay là sinh nhật của {data.UserName}! Gửi lời chúc ngay nào!",
                    };
                    Console.WriteLine(conversation);
                    //await db.UpdateConversationAsync(conversation);
                    int friendId = data.FriendId;
                    await hub.Clients.User(friendId.ToString())
                        .SendAsync("birthday", conversation);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[BirthDayConsumer Error]");
                    Console.WriteLine($"[BirthdayConsumer Error] {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer);
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    await Task.Delay(1000, stoppingToken);
            //}
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
                await _channel.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
