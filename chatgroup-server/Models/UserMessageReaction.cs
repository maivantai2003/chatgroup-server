using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class UserMessageReaction
    {
        [Key]
        public int ReactionId { get; set; }
        public int UserId { get; set; }
        public int UserMessageId { get; set; }
        public string? ReactionType { get; set; }
        public DateTime? ReactionDate { get; set; } = DateTime.Now;
        public int ReactionCount { get; set; } = 0;
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [ForeignKey(nameof(UserMessageId))] 
        public virtual UserMessages? userMessages { get; set; }
    }
}
