using chatgroup_server.Data;
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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();   
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
