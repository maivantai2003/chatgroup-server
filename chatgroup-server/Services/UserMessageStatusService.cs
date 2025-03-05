using chatgroup_server.Common;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Interfaces;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class UserMessageStatusService : IUserMessageStatusService
    {
        private readonly IUserMessageStatusRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UserMessageStatusService(IUserMessageStatusRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<UserMessageStatus>>> GetUserMessageStatusesAsync(int receiverId)
        {
            var result = await _repository.GetUserMessageStatusesAsync(receiverId);
            return ApiResponse<IEnumerable<UserMessageStatus>>.SuccessResponse("Lấy danh sách trạng thái tin nhắn", result);
        }

        public async Task<ApiResponse<UserMessageStatus>> GetUserMessageStatusByIdAsync(int id)
        {
            var result = await _repository.GetUserMessageStatusByIdAsync(id);
            return result != null
                ? ApiResponse<UserMessageStatus>.SuccessResponse("Lấy trạng thái tin nhắn thành công", result)
                : ApiResponse<UserMessageStatus>.ErrorResponse("Không tìm thấy trạng thái tin nhắn");
        }

        public async Task<ApiResponse<UserMessageStatus>> AddUserMessageStatusAsync(UserMessageStatus status)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.AddUserMessageStatusAsync(status);
                await _unitOfWork.CommitAsync();
                return ApiResponse<UserMessageStatus>.SuccessResponse("Thêm trạng thái tin nhắn thành công", status);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<UserMessageStatus>.ErrorResponse("Thêm trạng thái tin nhắn thất bại", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<UserMessageStatus>> UpdateUserMessageStatusAsync(UserMessageStatus status)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _repository.UpdateUserMessageStatus(status);
                await _unitOfWork.CommitAsync();
                return ApiResponse<UserMessageStatus>.SuccessResponse("Cập nhật trạng thái tin nhắn thành công", status);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<UserMessageStatus>.ErrorResponse("Cập nhật trạng thái tin nhắn thất bại", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserMessageStatusAsync(int id)
        {
            var status = await _repository.GetUserMessageStatusByIdAsync(id);
            if (status == null)
                return ApiResponse<bool>.ErrorResponse("Không tìm thấy trạng thái tin nhắn");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _repository.DeleteUserMessageStatus(status);
                await _unitOfWork.CommitAsync();
                return ApiResponse<bool>.SuccessResponse("Xóa trạng thái tin nhắn thành công", true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<bool>.ErrorResponse("Xóa trạng thái tin nhắn thất bại", new List<string> { ex.Message });
            }
        }
    }
}
