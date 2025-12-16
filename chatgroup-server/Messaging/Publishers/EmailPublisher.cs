using chatgroup_server.Messaging.Contracts;
using MassTransit;

namespace chatgroup_server.Messaging.Publishers
{
    public class EmailPublisher(IPublishEndpoint _publish)
    {
        public Task SendAsync(EmailMessage message)
        => _publish.Publish(message);
    }
}
