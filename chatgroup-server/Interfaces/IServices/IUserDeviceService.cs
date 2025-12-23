using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserDeviceService
    {
        Task<ApiResponse<bool>> AddUserDevice(UserDeviceAddDto userDeviceAddDto, string? ipAddress);
        Task<ApiResponse<UserDevice?>> GetUserDevice(int UserId, string DeviceId);
        Task<ApiResponse<bool>> UpdateUserDeviceAsync(UpdateUserDeviceDto userDeviceUpdateDto, string ipAddress);
        Task<List<string>> GetFcmTokensByUserIdAsync(int userId);
        Task<List<string>> GetFcmTokensByUserIdsAsync(List<int> userIds);
    }
}
