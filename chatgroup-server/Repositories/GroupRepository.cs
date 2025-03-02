using chatgroup_server.Data;
using chatgroup_server.Dtos;
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

        public async Task<IEnumerable<GroupUserDto>> GetAllGroupsAsync(int userId)
        {
            return await _context.GroupDetails.Where(x => x.UserId == userId && x.Status==1).AsNoTracking().Include(x => x.Group).ThenInclude(x=>x.groupDetail).ThenInclude(x=>x.User).Select(
                x => new GroupUserDto()
                {
                    GroupId = x.Group.GroupId,
                    GroupName=x.Group.GroupName ?? "None",
                    Status = x.Group.Status,
                    Avatar=x.Group.Avatar,
                    groupDetailUsers=x.Group.groupDetail.Select(
                        m=>new GroupDetailUserDto()
                        {
                            GroupDetailId = m.GroupDetailId,
                            UserId = m.UserId,  
                            AvatarUrl=m.User.Avatar,
                            Role=m.Role,
                            UserName=m.User.UserName,
                            Status=m.Status,
                        }).ToList(),
                    UserNumber = x.Group.groupDetail.Count()
                }).ToListAsync();
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
