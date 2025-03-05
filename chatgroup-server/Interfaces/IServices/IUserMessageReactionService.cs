using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserMessageReactionService
    {
        Task<ApiResponse<UserMessageReaction>> AddReactionAsync(UserMessageReaction reaction);
        Task<ApiResponse<IEnumerable<UserMessageReaction>>> GetReactionsByMessageIdAsync(int messageId);
    }
}
