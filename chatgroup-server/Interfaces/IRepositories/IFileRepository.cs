using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IFileRepository
    {
        Task CreateFileAsync(Files file);
        Task DeleteFileAsync(int id);
    }
}
