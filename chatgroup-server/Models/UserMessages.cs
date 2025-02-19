using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class UserMessages
    {
        [Key]
        public int UserMessageId {  get; set; }
        public int SenderId { get; set; }
        public int ReceiverId {  get; set; }
        public string MessageType { get; set; } = "Text";
        public string? Content {  get; set; }
        public DateTime CreateAt {  get; set; }=DateTime.Now;
        public int Status { get; set; } = 1;
        public ICollection<User>? users { get; set; }
    }
}
