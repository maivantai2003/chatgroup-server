using chatgroup_server.Common;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Interfaces;
using chatgroup_server.Models;
using chatgroup_server.Dtos;

namespace chatgroup_server.Services
{
    public class CloudMessageService : ICloudMessageService
    {
        private readonly ICloudMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CloudMessageService(ICloudMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<CloudMessageResponseDto>>> GetMessagesByUserIdAsync(int userId)
        {
            try
            {
                var messages = await _messageRepository.GetMessagesByUserIdAsync(userId);
                return ApiResponse<IEnumerable<CloudMessageResponseDto>>.SuccessResponse("Danh sách tin nhắn", messages);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<CloudMessageResponseDto>>.ErrorResponse("Lỗi khi lấy danh sách tin nhắn", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CloudMessage>> GetMessageByIdAsync(int id)
        {
            try
            {
                var message = await _messageRepository.GetMessageByIdAsync(id);
                if (message == null)
                {
                    return ApiResponse<CloudMessage>.ErrorResponse("Tin nhắn không tồn tại", new List<string>());
                }
                return ApiResponse<CloudMessage>.SuccessResponse("Chi tiết tin nhắn", message);
            }
            catch (Exception ex)
            {
                return ApiResponse<CloudMessage>.ErrorResponse("Lỗi khi lấy tin nhắn", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CloudMessage>> AddMessageAsync(CloudMessage message)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _messageRepository.AddMessageAsync(message);
                await _unitOfWork.CommitAsync();
                return ApiResponse<CloudMessage>.SuccessResponse("Gửi tin nhắn thành công", message);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<CloudMessage>.ErrorResponse("Gửi tin nhắn thất bại", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CloudMessage>> UpdateMessageAsync(CloudMessage message)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _messageRepository.UpdateMessage(message);
                await _unitOfWork.CommitAsync();
                return ApiResponse<CloudMessage>.SuccessResponse("Cập nhật tin nhắn thành công", message);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<CloudMessage>.ErrorResponse("Cập nhật tin nhắn thất bại", new List<string> { ex.Message });
            }
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null)
                return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _messageRepository.DeleteMessage(message);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
    }
}
