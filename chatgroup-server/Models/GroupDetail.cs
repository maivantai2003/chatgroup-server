using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class GroupDetail
    {
        [Key]
        public int GroupDetailId {  get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string Role { get; set; }
        public string Status {  get; set; }
    }
}
