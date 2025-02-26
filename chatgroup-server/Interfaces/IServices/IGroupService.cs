using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupService
    {
        Task<Group?> GetGroupByIdAsync(int groupId);
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<bool> AddGroupAsync(Group group);
        Task<bool> UpdateGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int groupId);
    }
}
