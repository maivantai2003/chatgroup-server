using chatgroup_server.Common;
using chatgroup_server.Dtos;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IDeviceVerificationService
    {
        Task<AuthResponse> VerifyDeviceAsync(VerifyOtpDto dto, string ipAddress);
    }
}
