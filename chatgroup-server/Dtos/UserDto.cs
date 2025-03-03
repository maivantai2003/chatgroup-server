namespace chatgroup_server.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
    }
}
