using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserDeviceRepository
    {
        Task AddUserDevice(UserDevice userDevice);
        Task UpdateUserDevice(UserDeviceUpdateDto userDevice);
        Task<UserDevice?> GetUserDevice(int UserId, string DeviceId);
    }
}
