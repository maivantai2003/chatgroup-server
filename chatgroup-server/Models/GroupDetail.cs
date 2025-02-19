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
        public User? user { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group? group { get; set; }

    }
}
