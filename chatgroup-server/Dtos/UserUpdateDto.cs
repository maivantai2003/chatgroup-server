namespace chatgroup_server.Dtos
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }
        public string Bio { get; set; } = "None";
        public string Sex { get; set; } = "Other";
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public DateTime Birthday { get; set; }
        public string? Avatar { get; set; }
        public string? CoverPhoto { get; set; }
        public int Status {  get; set; }
    }
}
