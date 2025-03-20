using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface ICloudMessageFileService
    {
        Task<ApiResponse<IEnumerable<CloudMessageFile>>> GetAllAsync();
        Task<ApiResponse<CloudMessageFile>> GetByIdAsync(int id);
        Task<ApiResponse<CloudMessageFileResponseDto>> AddAsync(CloudMessageFile cloudMessageFile);
        Task<ApiResponse<CloudMessageFile>> UpdateAsync(CloudMessageFile cloudMessageFile);
        Task<ApiResponse<IEnumerable<CloudMessageFileResponseDto>>> GetAllFileCloudMessage(int userId);
        Task<bool> DeleteAsync(int id);
    }
}
