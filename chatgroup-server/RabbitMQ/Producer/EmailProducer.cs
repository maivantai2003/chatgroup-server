using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace chatgroup_server.RabbitMQ.Producer
{
    public class EmailProducer
    {
        private readonly string _queueName = "email_queue";
        public async Task SendEmail(EmailMessageModel emailProducer)
        {
            var json=JsonConvert.SerializeObject(emailProducer);
            var body=Encoding.UTF8.GetBytes(json);
            var connection=await RabbitMQConnectionFactory.GetConnection();
            using var channel = await connection.CreateChannelAsync();
            var properties = new BasicProperties()
            {
                Persistent = true,
            };
            await channel.QueueDeclareAsync(queue:_queueName,durable:true,exclusive:false,autoDelete:false,arguments:null);
            await channel.BasicPublishAsync(exchange:string.Empty,routingKey:_queueName,mandatory:true,basicProperties:properties,body:body);

        }
    }
}
