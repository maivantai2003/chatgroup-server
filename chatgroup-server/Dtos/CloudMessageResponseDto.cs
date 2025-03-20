namespace chatgroup_server.Dtos
{
    public class CloudMessageResponseDto
    {
        public int CloudMessageId { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreateAt { get; set; }
        public string? Type { get; set; }
        public string UserName {  get; set; }
        public string AvatarUrl { get; set; }   
        public IEnumerable<CloudMessageFileDto>? Files { get; set; }
    }
}
