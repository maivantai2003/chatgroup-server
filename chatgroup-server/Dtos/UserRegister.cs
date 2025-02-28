using System.Text.Json.Serialization;

namespace chatgroup_server.Dtos
{
    public class UserRegister
    {
        public string Sex { get; set; } = "Other";
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime Birthday { get; set; } = new DateTime(1990, 1, 1);
        public string? Avatar { get; set; }
    }
}
