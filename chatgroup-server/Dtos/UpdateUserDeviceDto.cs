using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Dtos
{
    public class UpdateUserDeviceDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string DeviceId { get; set; } = null!;
        public string? DeviceToken { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? OS { get; set; }
        public string? DeviceName { get; set; }
        public string? Address { get; set; }
    }
}
