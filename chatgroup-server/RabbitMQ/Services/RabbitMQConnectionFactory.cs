using RabbitMQ.Client;
using System.Threading.Tasks;

namespace chatgroup_server.RabbitMQ.Services
{
    public static class RabbitMQConnectionFactory
    {
        //public static async Task<IConnection> GetConnection()
        //{
        //    var factory = new ConnectionFactory()
        //    {
        //        HostName = "localhost",

        //    };
        //    return await factory.CreateConnectionAsync();
        //}
        private static IConnection? _connection;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public static async Task<IConnection> GetConnectionAsync()
        {
            if (_connection != null && _connection.IsOpen)
                return _connection;

            await _lock.WaitAsync();
            try
            {
                if (_connection != null && _connection.IsOpen)
                    return _connection;

                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    //DispatchConsumersAsync = true 
                };

                _connection = await factory.CreateConnectionAsync();
                return _connection!;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
