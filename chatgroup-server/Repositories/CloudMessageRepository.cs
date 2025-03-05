using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class CloudMessageRepository : ICloudMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public CloudMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CloudMessage>> GetMessagesByUserIdAsync(int userId)
        {
            return await _context.CloudMessages
                .Include(m => m.User)
                .Include(m => m.CloudMessageFiles)
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreateAt)
                .ToListAsync();
        }

        public async Task<CloudMessage?> GetMessageByIdAsync(int id)
        {
            return await _context.CloudMessages
                .Include(m => m.User)
                .Include(m => m.CloudMessageFiles)
                .FirstOrDefaultAsync(m => m.CloudMessageId == id);
        }

        public async Task AddMessageAsync(CloudMessage message)
        {
            await _context.CloudMessages.AddAsync(message);
        }

        public void UpdateMessage(CloudMessage message)
        {
            _context.CloudMessages.Update(message);
        }

        public void DeleteMessage(CloudMessage message)
        {
            _context.CloudMessages.Remove(message);
        }
    }
}
