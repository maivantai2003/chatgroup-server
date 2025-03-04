namespace chatgroup_server.Dtos
{
    public class UserInfor
    {
        public int UserId { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; } = string.Empty;
        //public string Address {  get; set; }
        public string CoverPhoto { get; set; }
        public string Bio { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public string PhoneNumber { get; set; }
    }
}
