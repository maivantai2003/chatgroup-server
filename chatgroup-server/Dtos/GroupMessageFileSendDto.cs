namespace chatgroup_server.Dtos
{
    public class GroupMessageFileSendDto
    {
        public int GroupedMessageId { get; set; }
        public IEnumerable<GroupMessageFileDto>? Files { get; set; }
    }
}
