using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class UserMessages
    {
        [Key]
        public int UserMessageId {  get; set; }
        public int SenderId { get; set; }
        public int ReceiverId {  get; set; }
        public int? ReplyToMessageId { get; set; }
        public string MessageType { get; set; } = "Text";
        public string? Content {  get; set; }
        public DateTime CreateAt {  get; set; }=DateTime.Now;
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(SenderId))]
        public virtual User? Sender { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public virtual User? Receiver {  get; set; }
        [ForeignKey(nameof(ReplyToMessageId))]
        public virtual UserMessages? ReplyToMessage { get; set; }
        public virtual ICollection<UserMessages>? Replies { get; set; }
        public virtual ICollection<UserMessageReaction>? userMessageReactions { get; set; }
        public virtual ICollection<UserMessageStatus>? userMessageStatuses { get; set; }
        public virtual ICollection<UserMessageFile>? userMessageFiles { get; set; }
    }
}
