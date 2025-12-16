using chatgroup_server.Messaging.Contracts;
using MassTransit;

namespace chatgroup_server.Messaging.Publishers
{
    public class BirthdayPublisher(IPublishEndpoint _publish)
    {
        public Task PublishAsync(BirthdayMessage message)
        {
            return _publish.Publish(message);
        }
    }
}
