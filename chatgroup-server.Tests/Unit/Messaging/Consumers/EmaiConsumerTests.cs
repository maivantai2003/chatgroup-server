using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Messaging.Consumers;
using chatgroup_server.Messaging.Contracts;
using chatgroup_server.Models;
using FluentAssertions;
using MassTransit;
using Moq;

namespace chatgroup_server.Tests.Unit.Messaging.Consumers
{
    public class EmaiConsumerTests
    {
        [Fact]
        public async Task Consume_ValidEmailMessage_ShouldSendEmailToAllRecipients()
        {
            // Arrange
            var emailServiceMock = new Mock<ISendGmailService>();
            var consumer= new EmaiConsumer(emailServiceMock.Object);
            var message=new EmailMessage
            {
                Name = "Test",
                Subject = "Subject",
                Body = "Body",
                ToEmails = new List<string>
                {
                    "a@test.com",
                    "b@test.com"
                }
            };
            var context=Mock.Of<ConsumeContext<EmailMessage>>
                (x=>x.Message==message);
            //Act
            await consumer.Consume(context);
            // Assert
            emailServiceMock.Verify(g =>
                g.SendGmailAsync(It.Is<Gmail>(m =>
                    m.Subject == "Subject" &&
                    m.Body == "Body" &&
                    m.Name == "Test"
                )),
                Times.Exactly(2)
            );
        }
        [Fact]
        public async Task Consume_SendGmailThrowsException_ShouldThrow()
        {
            // Arrange
            var gmailServiceMock = new Mock<ISendGmailService>();
            gmailServiceMock
                .Setup(g => g.SendGmailAsync(It.IsAny<Gmail>()))
                .ThrowsAsync(new Exception("SMTP error"));

            var consumer = new EmaiConsumer(gmailServiceMock.Object);

            var message = new EmailMessage
            {
                ToEmails = new List<string> { "a@test.com" }
            };

            var context = Mock.Of<ConsumeContext<EmailMessage>>(c =>
                c.Message == message
            );
            // Act
            Func<Task> act = () => consumer.Consume(context);
            // Assert
            await act.Should().ThrowAsync<Exception>();
        }
    }
}
