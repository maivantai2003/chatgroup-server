using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupDetailService
    {
        Task<GroupDetail?> GetGroupDetailByIdAsync(int groupDetailId);
        Task<IEnumerable<GroupDetail>> GetGroupDetailsByGroupIdAsync(int groupId);
        Task<IEnumerable<GroupDetail>> GetGroupDetailsByUserIdAsync(int userId);
        Task<ApiResponse<GroupDetail>> AddGroupDetailAsync(GroupDetail groupDetail);
        Task<bool> UpdateGroupDetailAsync(GroupDetail groupDetail);
        Task<ApiResponse<GroupDetail>> DeleteGroupDetailAsync(int groupDetailId);
    }
}
