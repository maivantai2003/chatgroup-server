using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserMessageRepository
    {
        Task<IEnumerable<UserMessageDto>> GetAllUserMessageByIdAsync(int senderId, int receiverId);
        Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId);
        Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId);
        Task AddUserMessageAsync(UserMessages userMessage);
        void UpdateUserMessage(UserMessages userMessage);
        void DeleteUserMessage(UserMessages userMessage);
    }
}
