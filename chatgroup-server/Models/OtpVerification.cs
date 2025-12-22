using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class OtpVerification
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DeviceId { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
