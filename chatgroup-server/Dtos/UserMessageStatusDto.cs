namespace chatgroup_server.Dtos
{
    public class UserMessageStatusDto
    {
        public int UserMessageStatusId { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public bool IsReceived { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
