using chatgroup_server.Dtos;
using chatgroup_server.Models;
namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IGroupRepository
    {
        Task<GroupUserDto?> GetGroupByIdAsync(int groupId);
        Task<IEnumerable<GroupUserDto>> GetAllGroupsAsync(int userId);
        Task AddGroupAsync(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(Group group);
    }
}
