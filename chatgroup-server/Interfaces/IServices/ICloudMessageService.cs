using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface ICloudMessageService
    {
        Task<ApiResponse<IEnumerable<CloudMessageResponseDto>>> GetMessagesByUserIdAsync(int userId);
        Task<ApiResponse<CloudMessage>> GetMessageByIdAsync(int id);
        Task<ApiResponse<CloudMessageResponseDto>> AddMessageAsync(CloudMessage message);
        Task<ApiResponse<CloudMessage>> UpdateMessageAsync(CloudMessage message);
        Task<bool> DeleteMessageAsync(int id);
    }
}
