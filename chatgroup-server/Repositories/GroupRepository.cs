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

        public async Task<GroupUserDto?> GetGroupByIdAsync(int groupId)
        {
            return  await _context.Groups.AsNoTracking().Where(x=>x.GroupId==groupId && x.Status==1).Select(x=>new GroupUserDto()
            {
                GroupId = x.GroupId,
                GroupName = x.GroupName ?? "None",
                Status = x.Status,
                Avatar = x.Avatar,
                groupDetailUsers = x.groupDetail.Select(
                        m => new GroupDetailUserDto()
                        {
                            GroupDetailId = m.GroupDetailId,
                            UserId = m.UserId,
                            AvatarUrl = m.User.Avatar,
                            Role = m.Role,
                            UserName = m.User.UserName,
                            Status = m.Status,
                        }).ToList(),
                UserNumber = x.groupDetail.Count()
            }).FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<GroupUserDto>> GetAllGroupsAsync(int userId)
        {
            return await _context.GroupDetails.AsNoTracking().Where(x => x.UserId == userId && x.Status==1).Select(
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
