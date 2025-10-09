using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Humanizer;

namespace chatgroup_server.Services
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserDeviceRepository _userDeviceService;
        public UserDeviceService(IUnitOfWork unitOfWork, IUserDeviceRepository userDeviceService)
        {
            _unitOfWork = unitOfWork;
            _userDeviceService = userDeviceService;
        }

        public async Task<ApiResponse<bool>> AddUserDevice(UserDeviceAddDto userDeviceAddDto, string? ipAddress)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userDevice = new UserDevice()
                {
                    UserId = userDeviceAddDto.UserId,
                    DeviceToken = userDeviceAddDto.DeviceToken,
                    DeviceType = userDeviceAddDto.DeviceType,
                    Browser = userDeviceAddDto.Browser,
                    OS = userDeviceAddDto.OS,
                    DeviceName = userDeviceAddDto.DeviceName,
                    //Address = userDeviceAddDto.Address,
                    IpAddress = ipAddress,
                    LastLoginAt = DateTime.UtcNow,
                    LastActiveAt = DateTime.UtcNow,
                    IsOnline = true
                };
                await _userDeviceService.AddUserDevice(userDevice);
                await _unitOfWork.CommitAsync();
                return ApiResponse<bool>.SuccessResponse("Thêm Thành Công", true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<bool>.ErrorResponse("Thêm Không Thành Công", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
