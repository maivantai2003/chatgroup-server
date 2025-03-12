using chatgroup_server.Models;
using chatgroup_server.Dtos;
using chatgroup_server.Common;
namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupService
    {
        Task<ApiResponse<GroupUserDto?>> GetGroupByIdAsync(int groupId);
        Task<ApiResponse<IEnumerable<GroupUserDto>>> GetAllGroupsAsync(int userId);
        Task<ApiResponse<Group>> AddGroupAsync(Group group);
        Task<ApiResponse<Group>> UpdateGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int groupId);
    }
}
