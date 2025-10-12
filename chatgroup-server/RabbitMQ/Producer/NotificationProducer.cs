using chatgroup_server.RabbitMQ.Interfaces;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;

namespace chatgroup_server.RabbitMQ.Producer
{
    public class NotificationProducer:INotificationProducer
    {
        private readonly string QueueName = "notification_queue";
        //private IChannel? _channel;
        private readonly IRabbitMQChannelFactory _channelFactory;
        //public async Task InitializeAsync()
        //{
        //    var connection = await RabbitMQConnectionFactory.GetConnectionAsync();
        //    _channel = await connection.CreateChannelAsync();

        //    await _channel.QueueDeclareAsync(
        //        queue: _queueName,
        //        durable: true,
        //        exclusive: false,
        //        autoDelete: false,
        //        arguments: null);
        //}
        public NotificationProducer(IRabbitMQChannelFactory channelFactory)
        {
            _channelFactory = channelFactory;
        }
        public async Task SendNotificationAsync(NotificationMessageModel notification)
        {
            //if(_channel== null)
            //{
            //     await InitializeAsync();
            //}
            using var channel = await _channelFactory.CreateChannelAsync();
            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var json = JsonConvert.SerializeObject(notification);
            var body = Encoding.UTF8.GetBytes(json);
            var props = new BasicProperties()
            {
                Persistent = true
            };

            await channel!.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: QueueName,
                mandatory: false,
                basicProperties: props,
                body: body);
        }
    }
}
