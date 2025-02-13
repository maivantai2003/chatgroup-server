using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class User
    {
        [Key]
        public int UserId {  get; set; }
        public string Bio { get; set; } = "None";
        public string Sex { get; set; } = "Other";
        public string ?UserName { get; set; }
        public string ?PhoneNumber {  get; set; }
        public DateTime Birthday { get; set; } = new DateTime(1990, 1, 1);
        public string? Avatar {  get; set; }
        public string? CoverPhoto {  get; set; }
        public int Status { get; set; } = 1;
    }
}
