using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class GroupMessageFile
    {
        [Key]
        public int GroupMessageFileId { get; set; }
        public int FileId { get; set; }
        public int GroupedMessageId {  get; set; }
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(FileId))]
        public Files? Files { get; set; }
        [ForeignKey(nameof(GroupedMessageId))]
        public GroupMessages? groupMessages { get; set; }
    }
}
