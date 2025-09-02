namespace chatgroup_server.RabbitMQ.Models
{
    public class EmailMessageModel
    {
        public List<string> ToEmails { get; set; } = new();
        public string ?Subject { get; set; }
        public string ?Body { get; set; }
        public string ?Name { get; set; }
    }
}
