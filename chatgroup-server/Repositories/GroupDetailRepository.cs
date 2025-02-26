using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class GroupDetailRepository:IGroupDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupDetail?> GetGroupDetailByIdAsync(int groupDetailId)
        {
            return await _context.GroupDetails
                .Include(gd => gd.User)
                .Include(gd => gd.Group)
                .FirstOrDefaultAsync(gd => gd.GroupDetailId == groupDetailId);
        }

        public async Task<IEnumerable<GroupDetail>> GetGroupDetailsByGroupIdAsync(int groupId)
        {
            return await _context.GroupDetails
                .Include(gd => gd.User)
                .Where(gd => gd.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GroupDetail>> GetGroupDetailsByUserIdAsync(int userId)
        {
            return await _context.GroupDetails
                .Include(gd => gd.Group)
                .Where(gd => gd.UserId == userId)
                .ToListAsync();
        }

        public async Task AddGroupDetailAsync(GroupDetail groupDetail)
        {
            await _context.GroupDetails.AddAsync(groupDetail);
        }

        public void UpdateGroupDetail(GroupDetail groupDetail)
        {
            _context.GroupDetails.Update(groupDetail);
        }

        public void DeleteGroupDetail(GroupDetail groupDetail)
        {
            _context.GroupDetails.Remove(groupDetail);
        }
    }
}
