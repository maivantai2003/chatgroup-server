using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IFriendRepository
    {
        Task<Friends?> GetFriendshipAsync(int userId, int friendId);
        Task<IEnumerable<FriendRequest>> GetFriendsByUserIdAsync(int userId);
        Task<IEnumerable<FriendRequest>> GetFriendRequest(int friendId);
        Task<FriendRequest> GetFriend(int Id);
        Task AddFriendAsync(Friends friend);
        void UpdateFriend(Friends friend);
        void DeleteFriend(Friends friend);
    }
}
