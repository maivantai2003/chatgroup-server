namespace chatgroup_server.Dtos
{
    public class FriendRequest
    {
        public int Id { get; set; } 
        public int FriendId { get; set; }
        public int UserId { get; set; }
        public string Avatar {  get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Status {  get; set; } = 0;
    }
}
