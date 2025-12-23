using MassTransit;

namespace chatgroup_server.Messaging.Publishers
{
    public class SystemNotificationPublisher
    {
        private readonly IPublishEndpoint _publish;
        public SystemNotificationPublisher(IPublishEndpoint publish)
        {
            _publish = publish;
        }
        public Task SendAsync(Contracts.SystemNotificationMessage message)
            => _publish.Publish(message);
    }
}
