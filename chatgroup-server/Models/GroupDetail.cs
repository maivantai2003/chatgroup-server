using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class GroupDetail
    {
        [Key]
        public int GroupDetailId {  get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string? Role { get; set; } = "MemBer";
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual Group? Group { get; set; }

    }
}
