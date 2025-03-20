using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupMessageFileRepository
    {
        Task<IEnumerable<GroupMessageFile>> GetAllAsync();
        Task<GroupMessageFile?> GetByIdAsync(int id);
        Task AddAsync(GroupMessageFile groupMessageFile);
        Task<GroupMessageFileResponseDto> GetGroupMessageFile(int groupMessageFileId);
        Task<IEnumerable<GroupMessageFileResponseDto>> GetAllFileGroupMessage(int groupId);
        void Update(GroupMessageFile groupMessageFile);
        void Delete(GroupMessageFile groupMessageFile);
    }
}
