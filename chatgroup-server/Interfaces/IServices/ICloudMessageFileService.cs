using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface ICloudMessageFileService
    {
        Task<ApiResponse<IEnumerable<CloudMessageFile>>> GetAllAsync();
        Task<ApiResponse<CloudMessageFile>> GetByIdAsync(int id);
        Task<ApiResponse<CloudMessageFile>> AddAsync(CloudMessageFile cloudMessageFile);
        Task<ApiResponse<CloudMessageFile>> UpdateAsync(CloudMessageFile cloudMessageFile);
        Task<bool> DeleteAsync(int id);
    }
}
