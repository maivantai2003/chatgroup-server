namespace chatgroup_server.Dtos
{
    public class GroupMessageReactionDto
    {
        public int ReactionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ReactionType { get; set; }
    }
}
