using chatgroup_server.Data;
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
            return await _context.Friends
                .Include(f => f.User)
                .Include(f => f.Friend)
                .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                          (f.UserId == friendId && f.FriendId == userId));
        }

        public async Task<IEnumerable<Friends>> GetFriendsByUserIdAsync(int userId)
        {
            return await _context.Friends
                .Include(f => f.User)
                .Where(f => f.UserId == userId && f.Status == 1)
                .ToListAsync();
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
    }
}
