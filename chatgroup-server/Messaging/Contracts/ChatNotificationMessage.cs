namespace chatgroup_server.Messaging.Contracts
{
    public class ChatNotificationMessage
    {
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Type { get; set; }
        public string? DataJson { get; set; }
    }
}
