namespace chatgroup_server.RabbitMQ.Models
{
    public class NotificationMessageModel
    {
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Type { get; set; }
        public string? DataJson { get; set; }
    }
}
