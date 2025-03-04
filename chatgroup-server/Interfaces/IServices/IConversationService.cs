using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IConversationService
    {
        Task<ApiResponse<Conversation>> AddConversationAsync(Conversation conversation);
        Task<ApiResponse<Conversation?>> GetConversationByIdAsync(int id);
        Task<ApiResponse<Conversation>> UpdateConversationAsync(Conversation conversation);
        Task<ApiResponse<IEnumerable<Conversation>>> GetAllConversation(int userId);
        Task<bool> DeleteConversationAsync(int id);
    }
}
