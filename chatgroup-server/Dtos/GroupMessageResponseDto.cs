namespace chatgroup_server.Dtos
{
    public class GroupMessageResponseDto
    {
        public int GroupedMessageId { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderAvatar { get; set; }
        public int GroupId { get; set; }
        public int? ReplyToMessageId { get; set; }
        public string? Content { get; set; }
        public string? MessageType { get; set; }
        public DateTime CreateAt { get; set; }
        public int Status { get; set; } = 1;
        //public GroupMessageReplyDto? ReplyToMessage { get; set; }
        //public GroupMessageReplyDto? ReplyToMessage { get; set; }
        public List<GroupMessageFileDto>? Files { get; set; }
        //public List<GroupMessageReactionDto>? GroupMessageReactions { get; set; }
        //public List<GroupMessageStatusDto>? GroupMessageStatuses { get; set; }
    }
}
