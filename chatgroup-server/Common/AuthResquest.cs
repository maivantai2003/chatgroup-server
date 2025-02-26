using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Common
{
    public class AuthResquest
    {
        [Required]
        public string? PhoneNumber { get; set; }
    }
}
