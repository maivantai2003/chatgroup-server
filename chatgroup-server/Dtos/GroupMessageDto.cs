namespace chatgroup_server.Dtos
{
    public class GroupMessageDto
    {
        public int SenderId { get; set; }
        public int GroupId { get; set; }
        public int? ReplyToMessageId { get; set; }
        public string MessageType { get; set; } = "Text";
        public string? Content { get; set; } 
    }
}
