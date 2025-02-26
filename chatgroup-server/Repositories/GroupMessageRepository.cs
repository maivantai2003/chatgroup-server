using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class GroupMessageRepository:IGroupMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupMessages>> GetAllMessagesAsync()
        {
            return await _context.GroupMessages
                                 .Include(gm => gm.Sender)
                                 .Include(gm => gm.Group)
                                 .ToListAsync();
        }

        public async Task<GroupMessages?> GetMessageByIdAsync(int id)
        {
            return await _context.GroupMessages
                                 .Include(gm => gm.Sender)
                                 .Include(gm => gm.Group)
                                 .FirstOrDefaultAsync(gm => gm.GroupedMessageId == id);
        }

        public async Task AddMessageAsync(GroupMessages message)
        {
            await _context.GroupMessages.AddAsync(message);
        }

        public void UpdateMessage(GroupMessages message)
        {
            _context.GroupMessages.Update(message);
        }

        public void DeleteMessage(GroupMessages message)
        {
            _context.GroupMessages.Remove(message);
        }
    }
}
