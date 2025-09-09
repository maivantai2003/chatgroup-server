namespace chatgroup_server.Dtos
{
    public class ReCaptchaResponse
    {
        public bool success { get; set; }
        public List<string> ?error_codes { get; set; }
    }
}
