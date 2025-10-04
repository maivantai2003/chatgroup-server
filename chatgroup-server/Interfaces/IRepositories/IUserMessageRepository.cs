using chatgroup_server.Dtos;
using chatgroup_server.Models;
using System;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserMessageRepository
    {
        Task<IEnumerable<UserMessageResponseDto>> GetAllUserMessageByIdAsync(int senderId, int receiverId, DateTime? lastCreateAt = null, int pageSize = 10);
        Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId);
        Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId);
        Task<UserMessageResponseDto> GetUserMessageById(int userMessageId);
        Task AddUserMessageAsync(UserMessages userMessage);
        void UpdateUserMessage(UserMessages userMessage);
        void DeleteUserMessage(UserMessages userMessage);
    }
}
