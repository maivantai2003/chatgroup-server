using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class GroupMessageStatus
    {
        [Key]
        public int GroupMessageStatusId { get; set; }
        public int GroupMessageId { get; set; }
        public int ReceiverId { get; set; }
        public bool IsReceived { get; set; } = false;
        public bool IsRead { get; set; } = false;
        public DateTime? ReceivedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        [ForeignKey(nameof(GroupMessageId))]
        public virtual GroupMessages? GroupMessage { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public virtual User? Receiver { get; set; }
    }
}
