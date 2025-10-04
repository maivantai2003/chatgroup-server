using chatgroup_server.Common;
using chatgroup_server.Dtos;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserDeviceService
    {
        Task<ApiResponse<bool>> AddUserDevice(UserDeviceAddDto userDeviceAddDto, string? ipAddress);
    }
}
