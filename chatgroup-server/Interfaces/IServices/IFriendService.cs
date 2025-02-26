using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IFriendService
    {
        Task<Friends?> GetFriendshipAsync(int userId, int friendId);
        Task<IEnumerable<Friends>> GetFriendsByUserIdAsync(int userId);
        Task<bool> AddFriendAsync(Friends friend);
        Task<bool> UpdateFriendStatusAsync(Friends friend);
        Task<bool> DeleteFriendAsync(int friendId);
    }
}
