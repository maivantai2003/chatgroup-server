using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserMessageFileRepository
    {
        Task AddFileToMessageAsync(UserMessageFile file);
        Task<List<UserMessageFile>> GetFilesByMessageIdAsync(int messageId);
        Task<IEnumerable<UserMessageFileResponseDto>> GetAllFileUserMessage(int senderId, int receiverId);
        Task<UserMessageFileResponseDto> GetUserMessageFile(int userMessageFileId);
    }
}
