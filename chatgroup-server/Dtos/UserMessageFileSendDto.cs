namespace chatgroup_server.Dtos
{
    public class UserMessageFileSendDto
    {
        public int UserMessageId { get; set; }
        public IEnumerable<UserMessageFileDto>? Files { get; set; }
    }
}
