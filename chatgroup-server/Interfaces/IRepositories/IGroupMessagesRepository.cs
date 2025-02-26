using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupMessageRepository
    {
        Task<IEnumerable<GroupMessages>> GetAllMessagesAsync();
        Task<GroupMessages?> GetMessageByIdAsync(int id);
        Task AddMessageAsync(GroupMessages message);
        void UpdateMessage(GroupMessages message);
        void DeleteMessage(GroupMessages message);
    }
}
