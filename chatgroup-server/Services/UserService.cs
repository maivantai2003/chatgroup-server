using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using System.Collections.Generic;

namespace chatgroup_server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendGmailService _sendGmailService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IRedisService _redisService;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork,ISendGmailService sendGmailService, IJwtService jwtService, IConfiguration configuration,IRedisService redisService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _sendGmailService = sendGmailService;
            _jwtService = jwtService;
            _configuration = configuration;
            _redisService = redisService;
        }

        public async Task<ApiResponse<User>> AddUserAsync(User user)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _userRepository.AddUserAsync(user);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.Users(user.UserId));
                return ApiResponse<User>.SuccessResponse("Đăng Ký Thành Công",user);
            }
            catch (Exception ex) {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<User>.ErrorResponse("Đăng Ký Thất Bại",new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> CheckPhoneNumber(string? phoneNumber)
        {
            try
            {
                var result=await _userRepository.CheckPhoneNumber(phoneNumber);
                return ApiResponse<bool>.SuccessResponse("Số điện thoại"+(result?"đã":"chưa")+"tồn tại",result);
            }
            catch (Exception ex) {
                return ApiResponse<bool>.ErrorResponse("Lỗi khi tìm số điện thoại", new List<string>()
                {
                    ex.Message,
                });
            }
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return ApiResponse<string>.ErrorResponse(
                        "Số điện thoại và email không được để trống",
                        new List<string> { "Email không tồn tại" }
                    );
                }

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return ApiResponse<string>.ErrorResponse(
                        "Không tìm thấy người dùng với email này",
                        new List<string> { "Email không tồn tại" }
                    );
                }

                var token = _jwtService.GenerateResetPasswordToken(user.Gmail);
                var resetLink = $"{_configuration["FrontendUrl"]}/reset-password?token={token}";
                var body = $@"
                    <h3>Đặt lại mật khẩu</h3>
                    <p>Click vào link dưới để đặt lại mật khẩu:</p>
                    <a href='{resetLink}'>Đặt lại mật khẩu</a>
                    <p>Lưu ý: Link chỉ có hiệu lực trong 15 phút.</p>";

                var gmail = new Gmail
                {
                    Name = user.UserName ?? "Người dùng",
                    ToGmail = user.Gmail ?? request.Email,
                    Subject = "Đặt lại mật khẩu",
                    Body = body
                };

                await _sendGmailService.SendGmailAsync(gmail);

                return ApiResponse<string>.SuccessResponse(
                    "Đã gửi email đặt lại mật khẩu thành công",
                    "Vui lòng kiểm tra email để đặt lại mật khẩu."
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResponse(
                    "Gửi email đặt lại mật khẩu thất bại",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync(int userId)
        {
            string cacheKey=CacheKeys.Users(userId);
            try
            {
                var users = await _redisService.GetCacheAsync<IEnumerable<UserDto>>(cacheKey);
                if(users!=null && users.Any())
                {
                    return ApiResponse<IEnumerable<UserDto>>.SuccessResponse("Danh sách bạn có thể biết", users);
                }
                var result=await _userRepository.GetAllUsersAsync(userId);
                await _redisService.SetCacheAsync(cacheKey, result,TimeSpan.FromMinutes(CacheKeys.Time));
                return ApiResponse<IEnumerable<UserDto>>.SuccessResponse("Danh sách bạn có thể biết",result);
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResponse("Danh sách trống", new List<string>()
                {
                    ex.Message
                });
            }
            //return await _userRepository.GetAllUsersAsync();
        }

        public async Task<ApiResponse<UserInfor?>> GetUserById(int userId)
        {
            string cacheKey = CacheKeys.User(userId);
            try
            {
                var user = await _redisService.GetCacheAsync<UserInfor?>(cacheKey);
                if (user != null)
                {
                    return ApiResponse<UserInfor?>.SuccessResponse("Tìm thấy người dùng", user);
                }
                var result = await _userRepository.GetUserById(userId);
                await _redisService.SetCacheAsync(cacheKey, result,TimeSpan.FromMinutes(CacheKeys.Time));
                return ApiResponse<UserInfor?>.SuccessResponse("Tìm thấy người dùng", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserInfor?>.ErrorResponse("Không tìm thấy người dùng", new List<string>
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<User?>> GetUserByIdAsync(string numberPhone)
        {

            try
            {
                var result=await _userRepository.GetUserByIdAsync(numberPhone);
                return ApiResponse<User?>.SuccessResponse("Tìm thấy người dùng",result);
            }catch(Exception ex)
            {
                return ApiResponse<User?>.ErrorResponse("Không tìm thấy người dùng", new List<string>
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                string email = _jwtService.DecodeResetPasswordToken(request.Token).Email;
                if (string.IsNullOrEmpty(email))
                {
                    return ApiResponse<string>.ErrorResponse("Token không hợp lệ", new List<string>
                    {
                        "Không thể giải mã token"
                    });
                }
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return ApiResponse<string>.ErrorResponse("Không tìm thấy người dùng với email này", new List<string>
                    {
                        "Email không tồn tại"
                    });
                }
                await _userRepository.UpdateUserByEmail(request.NewPassword,email);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.User(user.UserId));
                return ApiResponse<string>.SuccessResponse("Đặt lại mật khẩu thành công", "User password: "+user.Password+" New Password: "+request.NewPassword+" Email: "+ email); 

            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<string>.ErrorResponse("Đặt lại mật khẩu thất bại", new List<string>
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<User>> UpdateUserAsync(User user)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _userRepository.UpdateUser(user);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.User(user.UserId));
                await _redisService.RemoveCacheAsync(CacheKeys.Users(user.UserId));
                return ApiResponse<User>.SuccessResponse("Cập nhật người dùng thành công", user);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<User>.ErrorResponse("Cập nhật người dùng không thành công", new List<string>
                {
                    ex.Message
                });
            }
        }

        public async Task<ApiResponse<bool>> UserUpdateStatus(int userId, UserUpdateStatusDto userUpdateStatusDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _userRepository.UpdateStatus(userId, userUpdateStatusDto);
                await _unitOfWork.CommitAsync();
                return ApiResponse<bool>.SuccessResponse("Cập nhật trạng thái thành công",result);
            }catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<bool>.ErrorResponse("Cập nhật không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
