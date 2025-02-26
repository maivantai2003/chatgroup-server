using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class UserMessageStatus
    {
        [Key]
        public int UserMessageStatusId { get; set; }
        public int UserMessageId {  get; set; }
        public int ReceiverId {  get; set; }
        public bool IsReceived { get; set; } = false;
        public bool IsRead { get; set; } = false;
        public DateTime? ReceivedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        [ForeignKey(nameof(UserMessageId))]
        public virtual UserMessages? Message { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public virtual User? Receiver { get; set; }
    }
}
