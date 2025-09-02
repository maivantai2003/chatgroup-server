using RabbitMQ.Client;

namespace chatgroup_server.RabbitMQ.Producer
{
    public class MessageProducer
    {
        private readonly string _queueName = "chat_queue";
    }
}
