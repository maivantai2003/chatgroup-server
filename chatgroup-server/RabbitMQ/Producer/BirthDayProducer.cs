using chatgroup_server.RabbitMQ.Models;
using chatgroup_server.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace chatgroup_server.RabbitMQ.Producer
{
    public class BirthDayProducer
    {
        private readonly string _exchangeName = "birthday.exchange";
        private IChannel? _channel;
        public async Task InitializeAsync()
        {
            var connection=await RabbitMQConnectionFactory.GetConnectionAsync();
            _channel=await connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange:_exchangeName,type:ExchangeType.Fanout,durable:true);
        }
        public async Task PublishBirthdayAsync(BirthdayModel model)
        {
            if (_channel == null)
            {
                await InitializeAsync();
            }
            var json=JsonConvert.SerializeObject(model);
            var body=Encoding.UTF8.GetBytes(json);
            await _channel.BasicPublishAsync(exchange:_exchangeName,routingKey:"",mandatory:false,basicProperties:new BasicProperties { Persistent=true},body:body);
        }
    }
}
