using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserMessageFileRepository
    {
        Task AddFileToMessageAsync(UserMessageFile file);
        Task<List<UserMessageFile>> GetFilesByMessageIdAsync(int messageId);
    }
}
