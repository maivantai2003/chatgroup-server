using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserMessageFileService
    {
        Task<ApiResponse<UserMessageFileResponseDto>> AddFileToMessageAsync(UserMessageFile file);
        Task<ApiResponse<IEnumerable<UserMessageFile>>> GetFilesByMessageIdAsync(int messageId);
        Task<ApiResponse<IEnumerable<UserMessageFileResponseDto>>> GetAllFileUserMessage(int senderId, int receiverId);
    }
}
