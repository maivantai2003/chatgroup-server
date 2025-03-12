using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Common;
using chatgroup_server.Dtos;

namespace chatgroup_server.Services
{
    public class UserMessageService:IUserMessageService
    {
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserMessageService(IUserMessageRepository userMessageRepository, IUnitOfWork unitOfWork)
        {
            _userMessageRepository = userMessageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId)
        {
            return await _userMessageRepository.GetMessagesBySenderIdAsync(senderId);
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId)
        {
            return await _userMessageRepository.GetMessagesByReceiverIdAsync(receiverId);
        }

        public async Task<ApiResponse<UserMessageResponseDto>> AddUserMessageAsync(UserMessages userMessage)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _userMessageRepository.AddUserMessageAsync(userMessage);
                await _unitOfWork.CommitAsync();
                var result=await _userMessageRepository.GetUserMessageById(userMessage.UserMessageId);
                if (result == null) {
                    return ApiResponse<UserMessageResponseDto>.ErrorResponse("Nhắn tin không thành công", new List<string>()
                    {
                        "Không tìm thấy tin nhắn"
                    });
                }
                return ApiResponse<UserMessageResponseDto>.SuccessResponse("Nhắn tin thành công", result);
            }
            catch (Exception ex)
            {
                {
                    await _unitOfWork.RollbackAsync();
                    return ApiResponse<UserMessageResponseDto>.ErrorResponse("Nhắn tin không thành công", new List<string>()
                {
                    ex.Message
                });
                }
            }
        }

        public async Task<bool> UpdateUserMessageAsync(UserMessages userMessage)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _userMessageRepository.UpdateUserMessage(userMessage);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteUserMessageAsync(int messageId)
        {
            //var userMessage = await _userMessageRepository.GetUserMessageByIdAsync(messageId);
            //if (userMessage == null) return false;

            //await _unitOfWork.BeginTransactionAsync();
            //try
            //{
            //    _userMessageRepository.DeleteUserMessage(userMessage);
            //    await _unitOfWork.CommitAsync();
            //    return true;
            //}
            //catch
            //{
            //    await _unitOfWork.RollbackAsync();
            //    return false;
            //}
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IEnumerable<UserMessageResponseDto>>> GetAllUserMessageByIdAsync(int senderId, int receiverId)
        {
            try
            {
                var result = await _userMessageRepository.GetAllUserMessageByIdAsync(senderId, receiverId);
                return ApiResponse<IEnumerable<UserMessageResponseDto>>.SuccessResponse("Lấy danh sách tin nhắn thành công", result);
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<UserMessageResponseDto>>.ErrorResponse("Lấy danh sách tin nhắn không thành công", new List<string>()
                {
                    ex.Message,
                });
            }
        }
    }
}
