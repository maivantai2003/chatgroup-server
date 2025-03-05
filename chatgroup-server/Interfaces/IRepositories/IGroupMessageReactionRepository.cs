using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupMessageReactionRepository
    {
        Task<IEnumerable<GroupMessageReaction>> GetAllAsync();
        Task<GroupMessageReaction?> GetByIdAsync(int id);
        Task AddAsync(GroupMessageReaction reaction);
        Task UpdateAsync(GroupMessageReaction reaction);
        Task DeleteAsync(int id);
    }
}
