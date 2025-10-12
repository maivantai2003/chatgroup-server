using chatgroup_server.RabbitMQ.Models;

namespace chatgroup_server.RabbitMQ.Interfaces
{
    public interface IEmailProducer
    {
        Task SendEmailAsync(EmailMessageModel email);
    }
}
