namespace chatgroup_server.Dtos
{
    public class ConversationUpdateGroupDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? UserSend { get; set; }
        public string? Content { get; set; }
    }
}
