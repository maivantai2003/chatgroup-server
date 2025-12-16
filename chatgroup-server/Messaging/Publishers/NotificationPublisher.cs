using chatgroup_server.Messaging.Contracts;
using MassTransit;

namespace chatgroup_server.Messaging.Publishers
{
    public class NotificationPublisher(IPublishEndpoint _publish)
    {
        public Task SendAsync(NotificationMessage message) => _publish.Publish(message);
    }
}
