using chatgroup_server.Data;
using chatgroup_server.Dtos;
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

        public async Task<IEnumerable<UserMessageFileResponseDto>> GetAllFileUserMessage(int senderId, int receiverId)
        {
           return await _context.UserMessages.AsNoTracking().Where(x => (x.SenderId == senderId && x.ReceiverId == receiverId)
            || (x.SenderId == receiverId && x.ReceiverId == senderId))
                .SelectMany(f=>f.userMessageFiles
                .Select(file=>new UserMessageFileResponseDto()
                {
                    UserId=f.SenderId,
                    FileId=file.FileId,
                    FileName=file.File.TenFile,
                    FileUrl=file.File.DuongDan,
                    TypeFile=file.File.LoaiFile,
                    SizeFile=file.File.KichThuocFile,
                    SentDate = f.CreateAt
                })).ToListAsync();  
        }

        public async Task<List<UserMessageFile>> GetFilesByMessageIdAsync(int messageId)
        {
            return await _context.UserMessageFiles.Include(f=>f.File)
                .Where(f => f.UserMessageId == messageId).ToListAsync();
        }

        public async Task<UserMessageFileResponseDto> GetUserMessageFile(int userMessageFileId)
        {
            return await _context.UserMessageFiles.AsNoTracking().Where(x => x.UserMessageFileId == userMessageFileId)
               .Select(f => new UserMessageFileResponseDto()
               {
                   UserId = f.userMessage.SenderId,
                   FileId = f.File.MaFile,
                   FileName = f.File.TenFile,
                   TypeFile = f.File.LoaiFile,
                   SizeFile = f.File.KichThuocFile,
                   FileUrl = f.File.DuongDan,
                   SentDate = f.userMessage.CreateAt
               }).FirstOrDefaultAsync();
        }
    }
}
