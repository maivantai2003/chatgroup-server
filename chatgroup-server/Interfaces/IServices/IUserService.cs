using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IServices
{
    public interface IUserService
    {
        Task<ApiResponse<User?>> GetUserByIdAsync(string numberPhone);
        Task<ApiResponse<UserInfor?>> GetUserById(int userId);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync(int userId);
        Task<ApiResponse<User>> AddUserAsync(User user);
        Task<ApiResponse<User>> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
