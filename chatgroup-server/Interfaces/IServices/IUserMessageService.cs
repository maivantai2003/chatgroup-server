using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserMessageService
    {
        Task<UserMessages?> GetUserMessageByIdAsync(int messageId);
        Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId);
        Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId);
        Task<bool> AddUserMessageAsync(UserMessages userMessage);
        Task<bool> UpdateUserMessageAsync(UserMessages userMessage);
        Task<bool> DeleteUserMessageAsync(int messageId);
    }
}
