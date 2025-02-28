using chatgroup_server.Common;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<ApiResponse<User>> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
