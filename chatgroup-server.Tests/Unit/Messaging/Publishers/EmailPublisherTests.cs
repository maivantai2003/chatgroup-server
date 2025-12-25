using chatgroup_server.Messaging.Contracts;
using chatgroup_server.Messaging.Publishers;
using MassTransit;
using Moq;
namespace chatgroup_server.Tests.Unit.Messaging.Publishers
{
    public class EmailPublisherTests
    {
        [Fact]
        public async Task SendAsync_ValidMessage_ShouldPublishMessage()
        {
            var publishMock = new Mock<IPublishEndpoint>();
            var emailPublisher = new EmailPublisher(publishMock.Object);
            var message = new EmailMessage
            {
                Name = "Test",
                Subject = "Hello",
                Body = "Test body",
                ToEmails = new List<string> { "a@test.com" }
            };
            await emailPublisher.SendAsync(message);
            publishMock.Verify(p =>p.Publish(message,It.IsAny<CancellationToken>()),Times.Once);
        }
    }
}
