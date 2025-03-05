using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class GroupMessageFileRepository : IGroupMessageFileRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupMessageFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupMessageFile>> GetAllAsync()
        {
            return await _context.GroupMessageFiles
                .Include(gmf => gmf.File)
                .Include(gmf => gmf.groupMessage)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<GroupMessageFile?> GetByIdAsync(int id)
        {
            return await _context.GroupMessageFiles
                .Include(gmf => gmf.File)
                .Include(gmf => gmf.groupMessage)
                .AsNoTracking()
                .FirstOrDefaultAsync(gmf => gmf.GroupMessageFileId == id);
        }

        public async Task AddAsync(GroupMessageFile groupMessageFile)
        {
            await _context.GroupMessageFiles.AddAsync(groupMessageFile);
        }

        public void Update(GroupMessageFile groupMessageFile)
        {
            _context.GroupMessageFiles.Update(groupMessageFile);
        }

        public void Delete(GroupMessageFile groupMessageFile)
        {
            _context.GroupMessageFiles.Remove(groupMessageFile);
        }
    }
}
