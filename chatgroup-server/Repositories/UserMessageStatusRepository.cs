using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class UserMessageStatusRepository : IUserMessageStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public UserMessageStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMessageStatus>> GetUserMessageStatusesAsync(int receiverId)
        {
            return await _context.UserMessageStatuses.Where(s => s.ReceiverId == receiverId).ToListAsync();
        }

        public async Task<UserMessageStatus?> GetUserMessageStatusByIdAsync(int id)
        {
            return await _context.UserMessageStatuses.FindAsync(id);
        }

        public async Task AddUserMessageStatusAsync(UserMessageStatus status)
        {
            await _context.UserMessageStatuses.AddAsync(status);
        }

        public void UpdateUserMessageStatus(UserMessageStatus status)
        {
            _context.UserMessageStatuses.Update(status);
        }

        public void DeleteUserMessageStatus(UserMessageStatus status)
        {
            _context.UserMessageStatuses.Remove(status);
        }
    }
}
