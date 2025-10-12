using chatgroup_server.RabbitMQ.Models;

namespace chatgroup_server.RabbitMQ.Interfaces
{
    public interface IBirthDayProducer
    {
        Task PublishBirthdayAsync(BirthdayModel model);
    }
}
