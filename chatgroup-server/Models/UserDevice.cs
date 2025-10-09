using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class UserDevice
    {
        [Key]
        public int UserDeviceId { get; set; }
        public int UserId { get; set; }
        public string? DeviceToken { get; set; }
        public string? DeviceType { get; set; }
        public bool IsOnline { get; set; } = false;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastActiveAt { get; set; }
        public string? IpAddress { get; set; }
        public string? Browser { get; set; }
        public string? OS { get; set; }
        public string? DeviceName { get; set; }
        public string? Address { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
