using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    public class CloudMessageFile
    {
        [Key]
        public int CloudMessageFileId {  get; set; }
        public int ClouMessageId {  get; set; }
        public int FileId {  get; set; }
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(ClouMessageId))]
        public CloudMessage? CloudMessage { get; set; }
        [ForeignKey(nameof(FileId))]
        public Files? Files { get; set; }
    }
}
