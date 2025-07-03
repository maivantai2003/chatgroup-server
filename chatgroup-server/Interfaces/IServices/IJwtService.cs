using chatgroup_server.Common;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IJwtService
    {
        Task<AuthResponse> GetTokenAsync(AuthResquest request, string ipAddress);
        Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string phoneNumber);
        Task<bool> IsTokenValid(string accessToken, string ipAddress);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
        Task<AuthResponse> GoogleLogin(string token, string ipAddress);
        string GenerateResetPasswordToken(string email);
        public (bool IsValid, string? Email, string? Reason) DecodeResetPasswordToken(string token);
    }
}
