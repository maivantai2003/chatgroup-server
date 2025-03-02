namespace chatgroup_server.Dtos
{
    public class GroupDetailUserDto
    {
        public int GroupDetailId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }   
        public string? Role { get; set; } = "MemBer";
        public int Status { get; set; } = 1;
    }
}
