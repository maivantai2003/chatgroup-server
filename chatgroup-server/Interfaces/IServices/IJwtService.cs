using chatgroup_server.Common;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IJwtService
    {
        Task<AuthResponse> GetTokenAsync(AuthResquest request, string ipAddress);
        Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string phoneNumber);
        Task<bool> IsTokenValid(string accessToken, string ipAddress);
    }
}
