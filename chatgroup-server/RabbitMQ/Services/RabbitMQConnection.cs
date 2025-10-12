using chatgroup_server.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace chatgroup_server.RabbitMQ.Services
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public RabbitMQConnection(IConfiguration config)
        {
            _factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:HostName"] ?? "localhost"
            };
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection != null && _connection.IsOpen)
                return _connection;

            await _lock.WaitAsync();
            try
            {
                if (_connection == null || !_connection.IsOpen)
                    _connection = await _factory.CreateConnectionAsync();
                return _connection;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
