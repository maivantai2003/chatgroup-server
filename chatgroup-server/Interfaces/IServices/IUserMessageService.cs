using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserMessageService
    {
        Task<ApiResponse<IEnumerable<UserMessageDto>>> GetAllUserMessageByIdAsync(int senderId, int receiverId);
        Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId);
        Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId);
        Task<ApiResponse<UserMessages>> AddUserMessageAsync(UserMessages userMessage);
        Task<bool> UpdateUserMessageAsync(UserMessages userMessage);
        Task<bool> DeleteUserMessageAsync(int messageId);
    }
}
