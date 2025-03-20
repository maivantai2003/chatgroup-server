using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupMessageFileService
    {
        Task<ApiResponse<IEnumerable<GroupMessageFile>>> GetAllAsync();
        Task<ApiResponse<GroupMessageFile>> GetByIdAsync(int id);
        Task<ApiResponse<GroupMessageFileResponseDto>> AddAsync(GroupMessageFile groupMessageFile);
        Task<ApiResponse<GroupMessageFile>> UpdateAsync(GroupMessageFile groupMessageFile);
        Task<ApiResponse<IEnumerable<GroupMessageFileResponseDto>>> GetAllFileGroupMessage(int groupId);
        Task<bool> DeleteAsync(int id);
    }
}
