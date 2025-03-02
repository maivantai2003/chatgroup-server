namespace chatgroup_server.Dtos
{
    public class FriendDto
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public int Status { get; set; } = 0;
    }
}
