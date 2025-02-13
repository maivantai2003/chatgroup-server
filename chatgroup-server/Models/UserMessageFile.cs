using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class UserMessageFile
    {
        [Key]
        public int UserMessageFileId { get; set; }
        public int FileId { get; set; }
        public int UserMessageId { get; set; }
        public int Status { get; set; } = 1;
    }
}
