namespace chatgroup_server.Dtos
{
    public class UserMessageFileDto
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string TypeFile {  get; set; }   
        public string SizeFile {  get; set; }
    }
}
