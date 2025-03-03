using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(string numberPhone);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(int userId);
        Task AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
    }
}
