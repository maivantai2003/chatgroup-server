using RabbitMQ.Client;

namespace chatgroup_server.RabbitMQ.Interfaces
{
    public interface IRabbitMQConnection
    {
        Task<IConnection> GetConnectionAsync();
    }
}
