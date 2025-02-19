using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class Friends
    {
        [Key]
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int FriendId {  get; set; }
        public int Status { get; set; } = 0;
        [ForeignKey(nameof(UserId))]
        public User? user { get; set; }
        [ForeignKey(nameof(FriendId))]
        public User? friend { get; set; }
        public DateTime CreateAt { get; set; }=DateTime.Now;
    }
}
