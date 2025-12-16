using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Messaging.Contracts;
using chatgroup_server.Models;
using MassTransit;

namespace chatgroup_server.Messaging.Consumers
{
    public class EmaiConsumer(ISendGmailService _gmail):IConsumer<EmailMessage>
    {
        public async Task Consume(ConsumeContext<EmailMessage> context)
        {
            var email= context.Message;
            var tasks = email.ToEmails.Select(to =>
                    _gmail.SendGmailAsync(new Gmail
                    {
                        ToGmail = to,
                        Subject = email.Subject,
                        Body = email.Body,
                        Name = email.Name
                    })
            );

            await Task.WhenAll(tasks);
        }
    }
}
