namespace chatgroup_server.Dtos
{
    public class UserMessageReactionDto
    {
        public int ReactionId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserAvatar { get; set; }
        public string? ReactionType { get; set; }
        public DateTime? ReactionDate { get; set; }
        public int ReactionCount { get; set; } = 0;
    }
}
