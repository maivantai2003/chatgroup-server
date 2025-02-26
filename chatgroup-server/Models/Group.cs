using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class Group
    {
        [Key]
        public int GroupId {  get; set; }
        public string ?GroupName {  get; set; }
        public string ?Avatar {  get; set; }
        public int Status { get; set; } = 1;
        public virtual ICollection<GroupDetail>? groupDetail { get; set; }
    }
}
