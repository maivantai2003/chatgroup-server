using chatgroup_server.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace chatgroup_server.RabbitMQ.Services
{
    public class RabbitMQChannelFactory:IRabbitMQChannelFactory
    {
        private readonly IRabbitMQConnection _connection;

        public RabbitMQChannelFactory(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            var conn = await _connection.GetConnectionAsync();
            return await conn.CreateChannelAsync();
        }
    }
}
