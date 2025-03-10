using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IFileService
    {
        Task<ApiResponse<Files>> CreateFile(Files file);
        Task<ApiResponse<Files>> DeleteFileAsync(int id);
    }
}
