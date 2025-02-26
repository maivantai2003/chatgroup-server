using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserMessageRepository
    {
        Task<UserMessages?> GetUserMessageByIdAsync(int messageId);
        Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId);
        Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId);
        Task AddUserMessageAsync(UserMessages userMessage);
        void UpdateUserMessage(UserMessages userMessage);
        void DeleteUserMessage(UserMessages userMessage);
    }
}
