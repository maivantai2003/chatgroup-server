using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class UserRefreshToken
    {
        [Key]
        public int RefreshTokenId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return ExpirationDate > DateTime.UtcNow;
            }
        }
        public string IpAddress { get; set; }
        public bool IsInvalidades { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
