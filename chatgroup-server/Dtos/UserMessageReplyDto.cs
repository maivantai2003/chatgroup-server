namespace chatgroup_server.Dtos
{
    public class UserMessageReplyDto
    {
        public int UserMessageId { get; set; }
        public string? Content { get; set; }
        public string MessageType { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
    }
}
