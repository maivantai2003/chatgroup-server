using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserMessageFileService
    {
        Task<ApiResponse<UserMessageFile>> AddFileToMessageAsync(UserMessageFile file);
        Task<ApiResponse<IEnumerable<UserMessageFile>>> GetFilesByMessageIdAsync(int messageId);
    }
}
