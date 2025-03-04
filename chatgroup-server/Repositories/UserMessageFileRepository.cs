using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class UserMessageFileRepository:IUserMessageFileRepository
    {
        private readonly ApplicationDbContext _context;

        public UserMessageFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFileToMessageAsync(UserMessageFile file)
        {
            await _context.UserMessageFiles.AddAsync(file);
        }

        public async Task<List<UserMessageFile>> GetFilesByMessageIdAsync(int messageId)
        {
            return await _context.UserMessageFiles.Include(f=>f.File)
                .Where(f => f.UserMessageId == messageId).ToListAsync();
        }
    }
}
