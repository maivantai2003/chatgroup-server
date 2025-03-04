using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> AddAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
            return conversation;
        }

        public async Task<Conversation?> GetByIdAsync(int id)
        {
            return await _context.Conversations.FindAsync(id);
        }

        public void Update(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
        }

        public void Delete(Conversation conversation)
        {
            _context.Conversations.Remove(conversation);
        }

        public async Task<IEnumerable<Conversation>> GetAllConversation(int userId)
        {
            return await _context.Conversations.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
