namespace chatgroup_server.RabbitMQ.Models
{
    public class BirthdayModel
    {
       public int SenderId {  get; set; }
       public int FriendId { get; set; }
       public string? UserName { get; set; }
    }
}
