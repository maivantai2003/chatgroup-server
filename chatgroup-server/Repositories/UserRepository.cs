using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(int userId)
        {
            var usersWithFriendStatus = _context.Friends
                .Where(f => f.Status == 0 || f.Status == 1)
                .Select(f => f.UserId)
                .Union(_context.Friends.Where(f => f.Status == 0 || f.Status == 1).Select(f => f.FriendId));

            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Status == 1 && u.UserId != userId && !usersWithFriendStatus.Contains(u.UserId))
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Avatar = u.Avatar
                })
                .ToListAsync();
        }


        public async Task<User?> GetUserByIdAsync(string numberPhone)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.PhoneNumber.Equals(numberPhone));
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
