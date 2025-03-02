namespace chatgroup_server.Dtos
{
    public class GroupUserDto
    {
        public int GroupId {  get; set; }
        public string GroupName { get; set; }
        public string? Avatar { get; set; }
        public int Status { get; set; } = 1;
        public int UserNumber { get; set; } = 0;
        public IEnumerable<GroupDetailUserDto>? groupDetailUsers { get; set; }
    }
}
