using chatgroup_server.Models;
namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupRepository
    {
        Task<Group?> GetGroupByIdAsync(int groupId);
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task AddGroupAsync(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(Group group);
    }
}
