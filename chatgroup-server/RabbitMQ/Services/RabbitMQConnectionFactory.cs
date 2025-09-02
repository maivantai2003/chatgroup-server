using RabbitMQ.Client;
using System.Threading.Tasks;

namespace chatgroup_server.RabbitMQ.Services
{
    public static class RabbitMQConnectionFactory
    {
        public static async Task<IConnection> GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",

            };
            return await factory.CreateConnectionAsync();
        }
    }
}
