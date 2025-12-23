using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Common
{
    public class AuthResquest
    {
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? UserName {  get; set; }
        public string DeviceId { get; set; } = null!;
        public string Browser { get; set; } = null!;
        public string OS { get; set; } = null!;
        public string DeviceName { get; set; } = null!;
    }
}
