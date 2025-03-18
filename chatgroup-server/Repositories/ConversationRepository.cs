using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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

        public async Task Update(Conversation conversation)
        {
            await _context.Conversations.Where(x => x.UserId == conversation.UserId && x.Id == conversation.Id && x.Type == conversation.Type).
                 ExecuteUpdateAsync(setters =>
                 setters.
                 SetProperty(x => x.LastMessage, DateTime.Now).
                 SetProperty(x => x.Content, conversation.Content).
                 SetProperty(x => x.UserSend, conversation.UserSend)
                 );
        }

        public void Delete(Conversation conversation)
        {
            _context.Conversations.Remove(conversation);
        }

        public async Task<IEnumerable<Conversation>> GetAllConversation(int userId)
        {
            return await _context.Conversations.AsNoTracking().Where(x => x.UserId == userId).OrderByDescending(x => x.LastMessage).ToListAsync();
        }

        public async Task UpdateInForConversation(ConversationUpdateInfor conversation)
        {
            await _context.Conversations.Where(x => x.Id == conversation.Id && x.Type == conversation.Type)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Avatar, conversation.Avatar).SetProperty(x => x.ConversationName, conversation.ConversationName));
        }

        public async Task UpdateConversationGroup(ConversationUpdateGroupDto conversationGroup)
        {
            await _context.Conversations.Where(x => x.Id == conversationGroup.Id && x.Type == conversationGroup.Type)
               .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.UserSend, conversationGroup.UserSend)
               .SetProperty(x => x.LastMessage, DateTime.Now)
               .SetProperty(x=>x.Content,conversationGroup.Content));
        }
    }
}
