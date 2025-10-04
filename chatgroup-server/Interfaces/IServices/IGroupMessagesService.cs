using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IGroupMessageService
    {
        Task<IEnumerable<GroupMessages>> GetAllMessagesAsync();
        Task<GroupMessages?> GetMessageByIdAsync(int id);
        Task<ApiResponse<GroupMessageResponseDto>> AddGroupMessage(GroupMessages groupMessage);
        Task<ApiResponse<IEnumerable<GroupMessageResponseDto>>> GetAllGroupMessageById(int groupId, DateTime? lastCreateAt = null, int pageSize = 10);
        Task<bool> SendMessageAsync(GroupMessages message);
        Task<bool> UpdateMessageAsync(GroupMessages message);
        Task<bool> DeleteMessageAsync(int id);
    }
}
