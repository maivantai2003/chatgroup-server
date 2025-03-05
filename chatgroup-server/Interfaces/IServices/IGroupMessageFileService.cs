using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupMessageFileService
    {
        Task<ApiResponse<IEnumerable<GroupMessageFile>>> GetAllAsync();
        Task<ApiResponse<GroupMessageFile>> GetByIdAsync(int id);
        Task<ApiResponse<GroupMessageFile>> AddAsync(GroupMessageFile groupMessageFile);
        Task<ApiResponse<GroupMessageFile>> UpdateAsync(GroupMessageFile groupMessageFile);
        Task<bool> DeleteAsync(int id);
    }
}
