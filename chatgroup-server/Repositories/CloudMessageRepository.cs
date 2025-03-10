using chatgroup_server.Data;
using chatgroup_server.Dtos;
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

        public async Task<IEnumerable<CloudMessageResponseDto>> GetMessagesByUserIdAsync(int userId)
        {
            return await _context.CloudMessages.AsNoTracking()
                .Where(m => m.UserId == userId)
                //.OrderByDescending(m => m.CreateAt)
                //.Skip((page - 1) * pageSize)
                //.Take(pageSize)
                .Select(x => new CloudMessageResponseDto()
                {
                    CloudMessageId = x.CloudMessageId,
                    UserId = userId,
                    CreateAt = x.CreateAt,
                    Type = x.Type,
                    Content = x.Content,    
                    UserName = x.User != null ? x.User.UserName : "Unknown",
                    AvatarUrl = x.User != null ? x.User.Avatar : null,
                    CloudMessageFile = x.CloudMessageFiles.Select(f => new FileDto()
                    {
                        TenFile = f.Files != null ? f.Files.TenFile : "No File",
                        DuongDan = f.Files != null ? f.Files.DuongDan : null,
                        LoaiFile = f.Files != null ? f.Files.LoaiFile : null,
                        KichThuocFile = f.Files != null ? f.Files.KichThuocFile : null,
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<CloudMessage?> GetMessageByIdAsync(int id)
        {
            return await _context.CloudMessages.FindAsync(id);
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

        public async Task<CloudMessageResponseDto?> GetCloudMessageByIdAsync(int id)
        {
            return await _context.CloudMessages.AsNoTracking().Where(x => x.CloudMessageId == id).Select(
                x => new CloudMessageResponseDto(){
                    CloudMessageId = x.CloudMessageId,
                    UserId = x.UserId,  
                    CreateAt=x.CreateAt,
                    Type = x.Type,
                    Content = x.Content,   
                    UserName=x.User!=null?x.User.UserName:"Unknown",
                    AvatarUrl=x.User!=null?x.User.UserName:null,
                    CloudMessageFile=x.CloudMessageFiles.Select(f=>new FileDto()
                    {
                        TenFile = f.Files != null ? f.Files.TenFile : "No File",
                        DuongDan = f.Files != null ? f.Files.DuongDan : null,
                        LoaiFile = f.Files != null ? f.Files.LoaiFile : null,
                        KichThuocFile = f.Files != null ? f.Files.KichThuocFile : null,
                    }).ToList()}).FirstOrDefaultAsync();
        }
    }
}
