namespace chatgroup_server.Dtos
{
    public class ConversationUpdateTimeDto
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string? Type { get; set; }
        public string? UserSend { get; set; }
        public string? Content { get; set; }
        public DateTime? lastMessage {  get; set; }=DateTime.Now;
    }
}
