using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserDeviceRepository
    {
        Task AddUserDevice(UserDevice userDevice);
    }
}
