using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class GroupMessageReaction
    {
        [Key]
        public int ReactionId { get; set; }
        public int UserId { get; set; }
        public int GroupedMessageId { get; set; }
        public string? ReactionType { get; set; }
        public DateTime? ReactionDate { get; set; } = DateTime.Now;
        public int ReactionCount { get; set; } = 0;
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [ForeignKey(nameof(GroupedMessageId))]
        public virtual GroupMessages? groupMessages { get; set; }
    }
}
