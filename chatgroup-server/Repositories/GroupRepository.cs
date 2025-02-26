using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class GroupRepository:IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Group?> GetGroupByIdAsync(int groupId)
        {
            return await _context.Groups
                .Include(g => g.groupDetail)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.groupDetail)
                .ToListAsync();
        }

        public async Task AddGroupAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
        }

        public void UpdateGroup(Group group)
        {
            _context.Groups.Update(group);
        }

        public void DeleteGroup(Group group)
        {
            _context.Groups.Remove(group);
        }
    }
}
