using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Repositories;

namespace chatgroup_server.Services
{
    public class UserMessageFileService : IUserMessageFileService
    {
        private readonly IUserMessageFileRepository _userMessageFileRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserMessageFileService(IUserMessageFileRepository userMessageFileRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userMessageFileRepository = userMessageFileRepository; 
        }
        public async Task<ApiResponse<UserMessageFileResponseDto>> AddFileToMessageAsync(UserMessageFile file)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _userMessageFileRepository.AddFileToMessageAsync(file);
                await _unitOfWork.CommitAsync();
                var result = await _userMessageFileRepository.GetUserMessageFile(file.UserMessageFileId);
                return ApiResponse<UserMessageFileResponseDto>.SuccessResponse("Thêm file thành công", result);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<UserMessageFileResponseDto>.ErrorResponse("Thêm file không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<IEnumerable<UserMessageFileResponseDto>>> GetAllFileUserMessage(int senderId, int receiverId)
        {
            try
            {
                var result=await _userMessageFileRepository.GetAllFileUserMessage(senderId, receiverId);
                return ApiResponse<IEnumerable<UserMessageFileResponseDto>>.SuccessResponse("Danh sách tin nhắn user", result);
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<UserMessageFileResponseDto>>.ErrorResponse("Lỗi khi lấy danh sách tin nhắn user", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<IEnumerable<UserMessageFile>>> GetFilesByMessageIdAsync(int messageId)
        {
            try
            {
                var files = await _userMessageFileRepository.GetFilesByMessageIdAsync(messageId);
                return ApiResponse<IEnumerable<UserMessageFile>>.SuccessResponse("Lấy file thành công", files);
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<UserMessageFile>>.ErrorResponse("Lấy file không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
