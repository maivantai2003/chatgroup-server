namespace chatgroup_server.Dtos
{
    public class UserUpdateStatusDto
    {
        public bool IsOnline { get; set; }
        public DateTime? FirstLogin { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
