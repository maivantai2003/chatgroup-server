
using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Hubs;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
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
        private readonly string _exchangeName = "birthday.exchange";
        private readonly string _queueName = "birthday.queue";
        public BirthDayConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var connection = await RabbitMQConnectionFactory.GetConnectionAsync();
            _channel = await connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange:_exchangeName,type:ExchangeType.Fanout,durable:true);
            await _channel.QueueDeclareAsync(queue:_queueName,durable:true,exclusive:false,autoDelete:true);
            await _channel.QueueBindAsync(queue:_queueName,exchange:_exchangeName,routingKey:"");
            await base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel == null)
                return;

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
                    Console.WriteLine($"[BirthdayConsumer Error] {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
                await _channel.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
