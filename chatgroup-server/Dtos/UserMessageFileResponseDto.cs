namespace chatgroup_server.Dtos
{
    public class UserMessageFileResponseDto
    {
        public int UserId {  get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string TypeFile { get; set; }
        public string SizeFile { get; set; }
        public DateTime SentDate { get; set; }
    }
}
