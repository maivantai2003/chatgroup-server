using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class CloudMessageFileRepository : ICloudMessageFileRepository
    {
        private readonly ApplicationDbContext _context;

        public CloudMessageFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CloudMessageFile>> GetAllAsync()
        {
            return await _context.CloudMessageFiles
                .Include(cmf => cmf.CloudMessage)
                .Include(cmf => cmf.Files)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CloudMessageFile?> GetByIdAsync(int id)
        {
            return await _context.CloudMessageFiles
                .Include(cmf => cmf.CloudMessage)
                .Include(cmf => cmf.Files)
                .FirstOrDefaultAsync(cmf => cmf.CloudMessageFileId == id);
        }

        public async Task AddAsync(CloudMessageFile cloudMessageFile)
        {
            await _context.CloudMessageFiles.AddAsync(cloudMessageFile);
        }

        public void Update(CloudMessageFile cloudMessageFile)
        {
            _context.CloudMessageFiles.Update(cloudMessageFile);
        }

        public void Delete(CloudMessageFile cloudMessageFile)
        {
            _context.CloudMessageFiles.Remove(cloudMessageFile);
        }

        public async Task<IEnumerable<CloudMessageFileResponseDto>> GetAllFileCloudMessage(int userId)
        {
            return await _context.CloudMessages.AsNoTracking().Where(x => x.UserId == userId)
                .SelectMany(f => f.CloudMessageFiles
                .Select(file=>new CloudMessageFileResponseDto()
                {
                    UserId=f.UserId,
                    FileId=file.FileId,
                    FileName=file.Files.TenFile,
                    TypeFile=file.Files.LoaiFile,
                    SizeFile=file.Files.KichThuocFile,
                    FileUrl=file.Files.DuongDan,
                    SentDate=f.CreateAt
                })).ToListAsync();   

        }

        public async Task<CloudMessageFileResponseDto> GetCloudMessageFile(int cloudMessageFileId)
        {
            return await _context.CloudMessageFiles.AsNoTracking().Where(x => x.CloudMessageFileId == cloudMessageFileId)
                .Select(f => new CloudMessageFileResponseDto()
                {
                    UserId = f.CloudMessage.UserId,
                    FileId = f.Files.MaFile,
                    FileName = f.Files.TenFile,
                    TypeFile = f.Files.LoaiFile,
                    SizeFile = f.Files.KichThuocFile,
                    FileUrl = f.Files.DuongDan,
                    SentDate = f.CloudMessage.CreateAt
                }).FirstOrDefaultAsync();
        }
    }
}
