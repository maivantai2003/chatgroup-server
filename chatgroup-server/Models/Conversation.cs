using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationId {  get; set; }
        public int? Id {  get; set; }
        public int? UserId {  get; set; }
        public string? Avatar {  get; set; }
        public string? UserSend {  get; set; }
        public string? ConversationName {  get; set; }   
        public DateTime LastMessage { get; set; }=DateTime.Now;
        public string? Type { get; set; }
        public string ?Content { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
