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
            //return await _context.CloudMessages
            //    .Include(m => m.User)
            //    .Include(m => m.CloudMessageFiles).ThenInclude(m=>m.Files)
            //    .Where(m => m.UserId == userId)
            //    .OrderByDescending(m => m.CreateAt).
            //    Select(x=>new CloudMessageResponseDto()
            //    {
            //        CloudMessageId=x.CloudMessageId,
            //        UserId = userId,
            //        CreateAt=x.CreateAt,
            //        Type = x.Type,
            //        UserName=x.User.UserName,
            //        AvatarUrl=x.User.Avatar,
            //        CloudMessageFile=x.CloudMessageFiles.Select(f => new FileDto()
            //        {
            //            TenFile=f.Files.TenFile,
            //            DuongDan=f.Files.DuongDan,
            //            KichThuocFile=f.Files.KichThuocFile,
            //            LoaiFile=f.Files.LoaiFile,  
            //        }).ToList()
            //    })
            //    .ToListAsync();
            return await _context.CloudMessages
        .Where(m => m.UserId == userId)
        .OrderByDescending(m => m.CreateAt)
        //.Skip((page - 1) * pageSize)
        //.Take(pageSize)
        .Select(x => new CloudMessageResponseDto()
        {
            CloudMessageId = x.CloudMessageId,
            UserId = userId,
            CreateAt = x.CreateAt,
            Type = x.Type,
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
