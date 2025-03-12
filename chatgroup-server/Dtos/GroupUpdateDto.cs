namespace chatgroup_server.Dtos
{
    public class GroupUpdateDto
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Avatar { get; set; }
        public int Status { get; set; } = 1;
    }
}
