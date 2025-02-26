using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class UserMessageFile
    {
        [Key]
        public int UserMessageFileId { get; set; }
        public int FileId { get; set; }
        public int UserMessageId { get; set; }
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(FileId))]
        public virtual Files? File { get; set; }
        [ForeignKey(nameof(UserMessageId))]
        public virtual UserMessages? userMessage { get; set; }
    }
}
