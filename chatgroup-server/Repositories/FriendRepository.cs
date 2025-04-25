using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class FriendRepository:IFriendRepository
    {
        private readonly ApplicationDbContext _context;
        public FriendRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Friends?> GetFriendshipAsync(int userId, int friendId)
        {
            return await _context.Friends.AsNoTracking()
                .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) || (f.UserId == friendId && f.FriendId == userId));
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendsByUserIdAsync(int userId)
        {
            return await _context.Friends.Include(f=>f.User).AsNoTracking().Where(f => (f.UserId == userId || f.FriendId == userId) && f.Status == 1)
                .Select(f => new FriendRequest()
                {
                    Id = f.Id,
                    FriendId = f.UserId == userId ? f.FriendId : f.UserId,
                    UserId = userId,
                    UserName = f.UserId == userId ? f.Friend!.UserName : f.User!.UserName,
                    Avatar = f.UserId == userId ? f.Friend!.Avatar : f.User!.Avatar,
                    CoverPhoto = f.UserId == userId ? f.Friend!.CoverPhoto : f.User!.CoverPhoto,
                    Bio = f.UserId == userId ? f.Friend!.Bio : f.User!.Bio,
                    Birthday = f.UserId == userId ? f.Friend!.Birthday : f.User!.Birthday,
                    Sex = f.UserId == userId ? f.Friend!.Sex : f.User!.Sex,
                    PhoneNumber = f.UserId == userId ? f.Friend!.PhoneNumber : f.User!.PhoneNumber,
                    //Address = f.UserId == userId ? f.Friend!.Address : f.User!.Address,
                    Status = f.Status
                }).ToListAsync();
        }
        public async Task AddFriendAsync(Friends friend)
        {
            await _context.Friends.AddAsync(friend);
        }

        public void UpdateFriend(Friends friend)
        {
            _context.Friends.Update(friend);
        }

        public void DeleteFriend(Friends friend)
        {
            _context.Friends.Remove(friend);
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequest(int friendId)
        {
            return await _context.Friends.Include(x => x.User).AsNoTracking().Where(x => x.FriendId == friendId && x.Status==0).Select(
                x=>new FriendRequest()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FriendId = x.FriendId,
                    UserName=x.User.UserName ?? "None",
                    Avatar = x.User.Avatar,
                    Status =x.Status
                }).ToListAsync();
        }

        public async Task<FriendRequest> GetFriend(int Id)
        {
            var result = await _context.Friends.Include(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            return new FriendRequest()
            {
                Id = result.Id,
                FriendId = result.FriendId,
                UserId = result.UserId,
                Avatar = result.User?.Avatar ?? string.Empty,
                UserName = result.User?.UserName ?? string.Empty,
                Status = result.Status
            };
        }

        public Task RemoveFriendAsync(int friendId)
        {
            throw new NotImplementedException();
        }
    }
}