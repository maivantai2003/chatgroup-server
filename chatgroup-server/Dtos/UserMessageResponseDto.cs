namespace chatgroup_server.Dtos
{
    public class UserMessageResponseDto
    {
        public int UserMessageId { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderAvatar { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverAvatar { get; set; }
        public int? ReplyToMessageId { get; set; }
        public string? MessageType { get; set; }
        public string? Content { get; set; }
        public DateTime CreateAt { get; set; }
        public int Status { get; set; } = 1;
        //public UserMessageReplyDto? ReplyToMessage { get; set; }
        //public List<UserMessageStatusDto>? MessageStatuses { get; set; }
        //public List<UserMessageReactionDto>? Reactions { get; set; }
        public List<UserMessageFileDto>? Files { get; set; }
    }
}
