namespace chatgroup_server.Dtos
{
    public class VerifyOtpDto
    {
        public string VerifyToken { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
