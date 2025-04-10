using chatgroup_server.Dtos;
using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(string numberPhone);
        Task<UserInfor?> GetUserById(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(int userId);
        Task<bool> CheckPhoneNumber(string? phoneNumber);
        Task AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
    }
}
