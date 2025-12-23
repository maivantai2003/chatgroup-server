namespace chatgroup_server.Common
{
    public class AuthResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string? Status { get; set; } = null!;
        public string? VerifyToken { get; set; } = null!;
        public bool RequireDeviceVerification { get; set; }
        public bool IsSuccess { get; set; }
        public string Reason { get; set; }
        public string? DeviceName { get; set; }
        public DateTime LoginTime { get; set; }=DateTime.Now;
        public string osName { get; set; }
        public string Browser { get; set; }
        public string ipAddress { get; set; }
    }
}
