namespace chatgroup_server.Interfaces.IServices
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string key, int length = 6, int expiryInMinutes = 5);
        Task<bool> ValidateOtpAsync(string key, string otp);
    }
}
