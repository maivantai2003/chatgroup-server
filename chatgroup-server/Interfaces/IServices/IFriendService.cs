using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IFriendService
    {
        Task<Friends?> GetFriendshipAsync(int userId, int friendId);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetFriendsByUserIdAsync(int userId);
        Task<ApiResponse<IEnumerable<FriendRequest>>> GetFriendRequest(int friendId);
        Task<ApiResponse<Friends>> AddFriendAsync(Friends friend);
        Task<ApiResponse<Friends>> UpdateFriendStatusAsync(Friends friend);
        Task<bool> DeleteFriendAsync(int friendId);
    }
}
