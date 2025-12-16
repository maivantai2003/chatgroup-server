namespace chatgroup_server.Messaging.Contracts
{
    public class BirthdayMessage
    {
        public int SenderId { get; set; }
        public int FriendId { get; set; }
        public string? UserName { get; set; }
    }
}
