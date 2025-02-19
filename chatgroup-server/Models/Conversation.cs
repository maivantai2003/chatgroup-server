using System.ComponentModel.DataAnnotations;

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
        public Group? group { get; set; }
        public User? user { get; set; }
    }
}
