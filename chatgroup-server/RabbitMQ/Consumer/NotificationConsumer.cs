
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.RabbitMQ.Interfaces;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace chatgroup_server.RabbitMQ.Consumer
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly string QueueName = "notification_queue";
        private IChannel? _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQConnection _connection;
        private readonly ILogger<NotificationConsumer> _logger;
        public NotificationConsumer(IServiceProvider serviceProvider,IRabbitMQConnection connection,ILogger<NotificationConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _connection = connection;
            _logger = logger;
        }
        //public override async Task StartAsync(CancellationToken cancellationToken)
        //{
        //    var connection = await RabbitMQConnectionFactory.GetConnectionAsync();
        //    _channel = await connection.CreateChannelAsync();

        //    await _channel.QueueDeclareAsync(
        //        queue: _queueName,
        //        durable: true,
        //        exclusive: false,
        //        autoDelete: false,
        //        arguments: null
        //    );

        //    await _channel.BasicQosAsync(0, 5, false);

        //    await base.StartAsync(cancellationToken);
        //}
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //if (_channel == null)
            //    return;
            var conn = await _connection.GetConnectionAsync();
            _channel = await conn.CreateChannelAsync();
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.BasicQosAsync(0, 3, false);
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var firebaseService = scope.ServiceProvider.GetRequiredService<IFirebaseService>();
                    var userDeviceService = scope.ServiceProvider.GetRequiredService<IUserDeviceService>();

                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonConvert.DeserializeObject<NotificationMessageModel>(json);

                    if (message == null) {
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                        return; 
                    }

                    //var fcmTokens = await userDeviceService.GetFcmTokensByUserIdAsync(message.UserId);
                    var fcmTokens= new List<string>() { "cqsxBb73rsswdOpaPuUE1E:APA91bG30GrBlwleo8iM8An9kfTlWbARb3XOimc5z-jQq8S6c_UZlLy2YzeqNQvTXUAOxyp4oK6nBTCX9kYgqJ6aEfDa1LisAhFXD05KjFGToDBLfKkqn9U", "dGXc6ddVo-LqsrJ4pQczFL:APA91bF-lMtZUMAAKa2wCxJRare3rbd4SNYIhJVZkdDke1zDhetRp0FG7wZuVC1OsSP4Zu88DEkhuYL8LHY4iYNo-hyRTdBXVDAsC8pLnp9lVzzoGxRAjbo" };
                    foreach (var token in fcmTokens)
                    {
                        await firebaseService.SendNotificationAsync(token, message.Title ?? "Thông báo", message.Body ?? "");
                    }
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[NotificationConsumer Error]");
                    Console.WriteLine($"[NotificationConsumer Error] {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

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
