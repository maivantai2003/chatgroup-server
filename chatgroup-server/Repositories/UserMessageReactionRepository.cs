using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class UserMessageReactionRepository:IUserMessageReactionRepository
    {
        private readonly ApplicationDbContext _context;

        public UserMessageReactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddReactionAsync(UserMessageReaction reaction)
        {
            await _context.UserMessageReactions.AddAsync(reaction);
        }

        public async Task<List<UserMessageReaction>> GetReactionsByMessageIdAsync(int messageId)
        {
            return await _context.UserMessageReactions.Include(r => r.User)
                .Where(r => r.UserMessageId == messageId)
                .ToListAsync();
        }
    }
}
