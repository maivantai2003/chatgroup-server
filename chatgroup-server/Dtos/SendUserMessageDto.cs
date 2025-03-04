namespace chatgroup_server.Dtos
{
    public class SendUserMessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageType { get; set; } = "Text";
        public string? Content { get; set; }
    }
}
