using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;
        public FileRepository(ApplicationDbContext context) { 
            _context = context;
        }
        public async Task CreateFileAsync(Files file)
        {
            await _context.Files.AddAsync(file);
        }

        public async Task DeleteFileAsync(int id)
        {
            await _context.Files.Where(x => x.MaFile == id).ExecuteUpdateAsync(setters =>
            setters.SetProperty(x=>x.TrangThai,0)
            );
        }
    }
}
