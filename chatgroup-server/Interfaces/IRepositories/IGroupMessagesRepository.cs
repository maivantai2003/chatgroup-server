using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupMessageRepository
    {
        Task<IEnumerable<GroupMessages>> GetAllMessagesAsync();
        Task<GroupMessageResponseDto?> GetGroupMessageByIdAsync(int id);
        Task<IEnumerable<GroupMessageResponseDto?>> GetAllGroupMessageById(int groupId, DateTime? lastCreateAt = null, int pageSize = 10);
        Task AddGroupMessageAsync(GroupMessages message);
        void UpdateMessage(GroupMessages message);
        void DeleteMessage(GroupMessages message);
    }
}
