using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class GroupMessages
    {
        [Key]
        public int GroupedMessageId { get; set; }
        public int SenderId { get; set; }
        public int GroupId {  get; set; }
        public string? Content {  get; set; }
        public string MessageType { get; set; } = "Text";
        public DateTime CreateAt { get; set; }=DateTime.Now;
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(SenderId))]
        public User? Sender { get; set; }
        [ForeignKey(nameof(GroupId))] 
        public Group? Group { get; set; }
        public virtual ICollection<GroupMessageFile> ?groupMessageFiles { get; set; }
        public virtual ICollection<GroupMessageReaction>? groupMessageReactions { get; set; }
        public virtual ICollection<GroupMessageStatus>? groupMessageStatuses { get; set; }
    }
}
