using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserMessageReactionRepository
    {
        Task AddReactionAsync(UserMessageReaction reaction);
        Task<List<UserMessageReaction>> GetReactionsByMessageIdAsync(int messageId);
    }
}
