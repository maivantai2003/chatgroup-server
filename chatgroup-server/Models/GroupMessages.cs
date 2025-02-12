using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class GroupMessages
    {
        [Key]
        public int GroupedMessageId { get; set; }
        public int SenderId { get; set; }

    }
}
