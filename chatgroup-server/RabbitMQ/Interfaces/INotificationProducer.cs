using chatgroup_server.RabbitMQ.Models;

namespace chatgroup_server.RabbitMQ.Interfaces
{
    public interface INotificationProducer
    {
        Task SendNotificationAsync(NotificationMessageModel notification);
    }
}
