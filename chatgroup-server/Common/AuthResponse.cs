namespace chatgroup_server.Common
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public string Reason { get; set; }
        public string? DeviceName { get; set; }
        public DateTime LoginTime { get; set; }=DateTime.Now;
    }
}
