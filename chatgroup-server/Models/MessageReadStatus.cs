using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class MessageReadStatus
    {
        [Key]
        public int MessageReadStatusId { get; set; }
        public int MessageId {  get; set; }
        public int UserId {  get; set; }
        public string MessageType { get; set; } = "User";
        public DateTime ReadAt { get; set; }=DateTime.Now;
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
