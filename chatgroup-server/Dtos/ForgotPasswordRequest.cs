namespace chatgroup_server.Dtos
{
    public class ForgotPasswordRequest
    {
        public string ?Email { get; set; }
        public string ?CaptchaToken { get; set; }
    }
}
