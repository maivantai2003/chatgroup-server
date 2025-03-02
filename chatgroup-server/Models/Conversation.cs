using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationId {  get; set; }
        public int GroupId {  get; set; }
        public int UserId {  get; set; }
        public DateTime LastMessage { get; set; }=DateTime.Now;
        public string ?Content { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group? Group { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
