using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserMessageStatusRepository
    {
        Task<IEnumerable<UserMessageStatus>> GetUserMessageStatusesAsync(int receiverId);
        Task<UserMessageStatus?> GetUserMessageStatusByIdAsync(int id);
        Task AddUserMessageStatusAsync(UserMessageStatus status);
        void UpdateUserMessageStatus(UserMessageStatus status);
        void DeleteUserMessageStatus(UserMessageStatus status);
    }
}
