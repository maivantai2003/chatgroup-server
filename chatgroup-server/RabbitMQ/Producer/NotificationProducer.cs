using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;

namespace chatgroup_server.RabbitMQ.Producer
{
    public class NotificationProducer
    {
        private readonly string _queueName = "notification_queue";
        private IChannel? _channel;
        public async Task InitializeAsync()
        {
            var connection = await RabbitMQConnectionFactory.GetConnectionAsync();
            _channel = await connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
        public async Task SendNotificationAsync(NotificationMessageModel notification)
        {
           if(_channel== null)
           {
                await InitializeAsync();
           }
            var json = JsonConvert.SerializeObject(notification);
            var body = Encoding.UTF8.GetBytes(json);
            var props = new BasicProperties()
            {
                Persistent = true
            };

            await _channel!.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _queueName,
                mandatory: false,
                basicProperties: props,
                body: body);
        }
    }
}
