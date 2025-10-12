using chatgroup_server.RabbitMQ.Interfaces;
using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace chatgroup_server.RabbitMQ.Producer
{
    public class BirthDayProducer:IBirthDayProducer
    {
        private readonly string ExchangeName = "birthday.exchange";
        private readonly IRabbitMQChannelFactory _channelFactory;
        public BirthDayProducer(IRabbitMQChannelFactory channelFactory)
        {
            _channelFactory = channelFactory;
        }
        public async Task PublishBirthdayAsync(BirthdayModel model)
        {
            using var channel = await _channelFactory.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(
                exchange:ExchangeName,
                type:ExchangeType.Fanout,
                durable:true
                );
            var props = new BasicProperties { Persistent = true };
            var json=JsonConvert.SerializeObject(model);
            var body=Encoding.UTF8.GetBytes(json);
            await channel.BasicPublishAsync(
                exchange:ExchangeName,
                routingKey:"",
                mandatory:false,
                basicProperties:props,
                body:body);
        }
    }
}
