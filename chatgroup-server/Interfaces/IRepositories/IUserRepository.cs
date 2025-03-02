using chatgroup_server.Models;

namespace chatgroup_server.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(string numberPhone);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
    }
}
