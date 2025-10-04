using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserMessageService
    {
        Task<ApiResponse<IEnumerable<UserMessageResponseDto>>> GetAllUserMessageByIdAsync(int senderId, int receiverId, DateTime? lastCreateAt = null, int pageSize = 10);
        Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId);
        Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId);
        Task<ApiResponse<UserMessageResponseDto>> AddUserMessageAsync(UserMessages userMessage);
        Task<bool> UpdateUserMessageAsync(UserMessages userMessage);
        Task<bool> DeleteUserMessageAsync(int messageId);
    }
}
