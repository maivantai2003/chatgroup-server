using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserMessageStatusService
    {
        Task<ApiResponse<IEnumerable<UserMessageStatus>>> GetUserMessageStatusesAsync(int receiverId);
        Task<ApiResponse<UserMessageStatus>> GetUserMessageStatusByIdAsync(int id);
        Task<ApiResponse<UserMessageStatus>> AddUserMessageStatusAsync(UserMessageStatus status);
        Task<ApiResponse<UserMessageStatus>> UpdateUserMessageStatusAsync(UserMessageStatus status);
        Task<ApiResponse<bool>> DeleteUserMessageStatusAsync(int id);
    }
}
