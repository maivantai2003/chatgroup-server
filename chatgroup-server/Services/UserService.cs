using chatgroup_server.Common;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<User>> AddUserAsync(User user)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _userRepository.AddUserAsync(user);
                await _unitOfWork.CommitAsync();
                return ApiResponse<User>.SuccessResponse("Đăng Ký Thành Công",user);
            }
            catch (Exception ex) {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<User>.ErrorResponse("Đăng Ký Thất Bại",new List<string> { ex.Message });
            }
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public Task<User?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
