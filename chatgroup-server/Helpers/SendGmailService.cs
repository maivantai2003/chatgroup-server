using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using MimeKit;
using MailKit.Net.Smtp;
namespace chatgroup_server.Helpers
{
    public class SendGmailService : ISendGmailService
    {
        public  async Task SendGmailAsync(Gmail gmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("vantaii12082003@gmail.com", "vantaii12082003@gmail.com"));
            message.To.Add(new MailboxAddress(gmail.name, gmail.ToGmail));
            message.Subject = gmail.Subject;

            // Thiết lập nội dung email
            message.Body = new TextPart("html")
            {
                Text = gmail.Body
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("vantaii12082003@gmail.com", "");
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
