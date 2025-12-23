using chatgroup_server.Common;
using chatgroup_server.Models;
using System.Security.Claims;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IJwtService
    {
        string GenerateVerifyToken(int userId, string deviceId);
        Task<AuthResponse> GetTokenAsync(AuthResquest request, string ipAddress);
        Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string phoneNumber);
        Task<bool> IsTokenValid(string accessToken, string ipAddress);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
        Task<AuthResponse> GoogleLogin(string token, string ipAddress);
        string GenerateResetPasswordToken(string email);
        (bool IsValid, int UserId, string DeviceId) ValidateVerifyToken(string token);
        public (bool IsValid, string? Email, string? Reason) DecodeResetPasswordToken(string token);
        Task<AuthResponse> LoginAfterDeviceVerifiedAsync(int userId, string ipAddress);
        //string GenerateAccessToken(User user);
        //string GenerateRefreshToken();

        //ClaimsPrincipal? ValidateAccessToken(string token);

        //string GenerateVerifyDeviceToken(int userId, string deviceId);
        //(bool IsValid, int UserId, string DeviceId) ValidateVerifyDeviceToken(string token);

        //string GenerateResetPasswordToken(string email);
        //(bool IsValid, string? Email, string? Reason) ValidateResetPasswordToken(string token);
    }
}
