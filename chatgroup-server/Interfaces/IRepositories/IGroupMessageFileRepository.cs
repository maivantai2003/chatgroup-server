using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupMessageFileRepository
    {
        Task<IEnumerable<GroupMessageFile>> GetAllAsync();
        Task<GroupMessageFile?> GetByIdAsync(int id);
        Task AddAsync(GroupMessageFile groupMessageFile);
        void Update(GroupMessageFile groupMessageFile);
        void Delete(GroupMessageFile groupMessageFile);
    }
}
