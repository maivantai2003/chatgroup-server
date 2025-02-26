using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupMessageService
    {
        Task<IEnumerable<GroupMessages>> GetAllMessagesAsync();
        Task<GroupMessages?> GetMessageByIdAsync(int id);
        Task<bool> SendMessageAsync(GroupMessages message);
        Task<bool> UpdateMessageAsync(GroupMessages message);
        Task<bool> DeleteMessageAsync(int id);
    }
}
