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
                    DeviceId = userDeviceAddDto.DeviceId,
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

        public async Task<List<string>> GetFcmTokensByUserIdAsync(int userId)
        {
            return await _userDeviceService.GetFcmTokensByUserIdAsync(userId);
        }

        public async Task<List<string>> GetFcmTokensByUserIdsAsync(List<int> userIds)
        {
            return await _userDeviceService.GetFcmTokensByUserIdsAsync(userIds);
        }

        public async Task<ApiResponse<UserDevice?>> GetUserDevice(int UserId, string DeviceId)
        {
            try
            {
                var response=await _userDeviceService.GetUserDevice(UserId, DeviceId);
                return ApiResponse<UserDevice?>.SuccessResponse("Lấy Thành Công", response);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDevice?>.ErrorResponse("Lấy Không Thành Công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<bool>> UpdateUserDeviceAsync(UpdateUserDeviceDto userDeviceUpdateDto, string ipAddress)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var response =await _userDeviceService.UpdateUserDevice(userDeviceUpdateDto, ipAddress);
                await _unitOfWork.CommitAsync();
                return ApiResponse<bool>.SuccessResponse("Cập Nhật Thành Công", true);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<bool>.ErrorResponse("Cập Nhật Không Thành Công", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
