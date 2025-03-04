namespace chatgroup_server.Dtos
{
    public class ConversationDto
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public string? Avatar { get; set; }
        public string? ConversationName { get; set; }
        public string? UserSend { get; set; }
        public string? Type { get; set; }
        public string? Content { get; set; }
    }
}
