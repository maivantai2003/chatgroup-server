using chatgroup_server.RabbitMQ.Interfaces;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace chatgroup_server.RabbitMQ.Producer
{
    public class EmailProducer:IEmailProducer
    {
        
        private readonly string QueueName = "email_queue";
        private readonly IRabbitMQChannelFactory _channelFactory;
        public EmailProducer(IRabbitMQChannelFactory channelFactory)
        {
            _channelFactory = channelFactory;
        }
       
        public async Task SendEmailAsync(EmailMessageModel email)
        {
            using var channel = await _channelFactory.CreateChannelAsync();
            await channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);

            var json = JsonConvert.SerializeObject(email);
            var body = Encoding.UTF8.GetBytes(json);
            var props = new BasicProperties { Persistent = true };

            await channel.BasicPublishAsync(string.Empty, QueueName, false, props, body);
        }
    }
}
