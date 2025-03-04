using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class CloudMessage
    {
        [Key]
        public int CloudMessageId {  get; set; }
        public int UserId {  get; set; }
        public string? Content { get; set; }
        public DateTime CreateAt { get; set; }=DateTime.Now;
        public string? Type {  get; set; }
        [ForeignKey(nameof(UserId))]
        public User ?User { get; set; }
        public ICollection<CloudMessageFile>? CloudMessageFiles { get; set; }
    }
}
