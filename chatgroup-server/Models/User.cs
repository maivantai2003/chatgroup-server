using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class User
    {
        [Key]
        public int UserId {  get; set; }
        public string ?UserName { get; set; }
        public string ?PhoneNumber {  get; set; }
        public string? Avatar {  get; set; }
        public int Status { get; set; } = 1;
    }
}
