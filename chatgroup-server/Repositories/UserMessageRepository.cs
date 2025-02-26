using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class UserMessageRepository:IUserMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public UserMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserMessages?> GetUserMessageByIdAsync(int messageId)
        {
            return await _context.UserMessages
                .Include(um => um.Sender)
                .Include(um => um.Receiver)
                .Include(um => um.userMessageReactions)
                .Include(um => um.userMessageStatuses)
                .Include(um => um.userMessageFiles)
                .FirstOrDefaultAsync(um => um.UserMessageId == messageId);
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesBySenderIdAsync(int senderId)
        {
            return await _context.UserMessages
                .Include(um => um.Receiver)
                .Where(um => um.SenderId == senderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserMessages>> GetMessagesByReceiverIdAsync(int receiverId)
        {
            return await _context.UserMessages
                .Include(um => um.Sender)
                .Where(um => um.ReceiverId == receiverId)
                .ToListAsync();
        }

        public async Task AddUserMessageAsync(UserMessages userMessage)
        {
            await _context.UserMessages.AddAsync(userMessage);
        }

        public void UpdateUserMessage(UserMessages userMessage)
        {
            _context.UserMessages.Update(userMessage);
        }

        public void DeleteUserMessage(UserMessages userMessage)
        {
            _context.UserMessages.Remove(userMessage);
        }
    }
}
