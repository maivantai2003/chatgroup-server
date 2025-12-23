using chatgroup_server.Messaging.Contracts;
using MassTransit;

namespace chatgroup_server.Messaging.Publishers
{
    public class ChatNotificationPublisher(IPublishEndpoint _publish)
    {
        public Task SendAsync(ChatNotificationMessage message) => _publish.Publish(message);
    }
}
