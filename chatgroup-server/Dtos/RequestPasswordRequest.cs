namespace chatgroup_server.Dtos
{
    public class RequestPasswordRequest
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
    }
}
