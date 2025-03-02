using chatgroup_server.Models;
using chatgroup_server.Dtos;
using chatgroup_server.Common;
namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupService
    {
        Task<Group?> GetGroupByIdAsync(int groupId);
        Task<ApiResponse<IEnumerable<GroupUserDto>>> GetAllGroupsAsync(int userId);
        Task<ApiResponse<Group>> AddGroupAsync(Group group);
        Task<bool> UpdateGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int groupId);
    }
}
