namespace chatgroup_server.Dtos
{
    public class GroupDetailDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string? Role { get; set; } = "MemBer";
    }
}
