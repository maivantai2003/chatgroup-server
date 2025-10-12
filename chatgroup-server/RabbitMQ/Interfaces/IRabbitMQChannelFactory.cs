using RabbitMQ.Client;

namespace chatgroup_server.RabbitMQ.Interfaces
{
    public interface IRabbitMQChannelFactory
    {
        Task<IChannel> CreateChannelAsync();
    }
}
