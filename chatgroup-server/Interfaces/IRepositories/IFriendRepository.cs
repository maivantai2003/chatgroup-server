using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IFriendRepository
    {
        Task<Friends?> GetFriendshipAsync(int userId, int friendId);
        Task<IEnumerable<Friends>> GetFriendsByUserIdAsync(int userId);
        Task AddFriendAsync(Friends friend);
        void UpdateFriend(Friends friend);
        void DeleteFriend(Friends friend);
    }
}
