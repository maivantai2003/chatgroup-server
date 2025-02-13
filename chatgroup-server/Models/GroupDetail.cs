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
        public string? Role { get; set; }
        public string? Status {  get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group? Group { get; set; }

    }
}
