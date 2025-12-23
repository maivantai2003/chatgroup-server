using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserDeviceRepository
    {
        Task AddUserDevice(UserDevice userDevice);
        Task<bool> UpdateUserDevice(UpdateUserDeviceDto userDevice,string ipAddress);
        Task<UserDevice?> GetUserDevice(int UserId, string DeviceId);
        Task<List<string>> GetFcmTokensByUserIdAsync(int userId);
        Task<List<string>> GetFcmTokensByUserIdsAsync(List<int> userIds);

    }
}
