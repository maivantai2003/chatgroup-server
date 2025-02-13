using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class GroupMembers
    {
        [Key]
        public int Id { get; set; }
        public int GroupId {  get; set; }
        public int UserId {  get; set; }
        public DateTime CreateAt { get; set; }=DateTime.Now;
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(GroupId))]
        public Group? Group { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
