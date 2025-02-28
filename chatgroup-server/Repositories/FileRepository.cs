using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using CloudinaryDotNet;

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

    }
}
