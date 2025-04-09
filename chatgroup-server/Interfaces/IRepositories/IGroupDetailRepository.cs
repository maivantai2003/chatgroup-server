using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupDetailRepository
    {
        Task<GroupDetail?> GetGroupDetailByIdAsync(int groupDetailId);
        Task<IEnumerable<GroupDetail>> GetGroupDetailsByGroupIdAsync(int groupId);
        Task<IEnumerable<GroupDetail>> GetGroupDetailsByUserIdAsync(int userId);
        Task AddGroupDetailAsync(GroupDetail groupDetail);
        void UpdateGroupDetail(GroupDetail groupDetail);
        Task DeleteGroupDetail(LeaveGroupDetailDto groupDetailDto);
    }
}
