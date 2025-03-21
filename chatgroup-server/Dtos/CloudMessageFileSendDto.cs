namespace chatgroup_server.Dtos
{
    public class CloudMessageFileSendDto
    {
        public int CloudMessageId {  get; set; }
        public IEnumerable<CloudMessageFileDto>? Files { get; set; }
    }
}
