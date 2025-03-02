using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class FriendService:IFriendService
    {
        private readonly IFriendRepository _friendsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public FriendService(IFriendRepository friendsRepository, IUnitOfWork unitOfWork)
        {
            _friendsRepository = friendsRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Friends?> GetFriendshipAsync(int userId, int friendId)
        {
            return await _friendsRepository.GetFriendshipAsync(userId, friendId);
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetFriendsByUserIdAsync(int userId)
        {
            try
            {
                var result=await _friendsRepository.GetFriendsByUserIdAsync(userId);
                if (result != null)
                {
                    return ApiResponse<IEnumerable<FriendRequest>>.SuccessResponse("Danh sách bạn bè",result);
                }
                return ApiResponse<IEnumerable<FriendRequest>>.SuccessResponse("Danh sách bạn bè trống", result);
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<FriendRequest>>.ErrorResponse("Danh sách bạn bè", new List<string>
                {
                    ex.Message
                });
            }
        }
        public async Task<ApiResponse<Friends>> AddFriendAsync(Friends friend)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _friendsRepository.AddFriendAsync(friend);
                await _unitOfWork.CommitAsync();
                return ApiResponse<Friends>.SuccessResponse("Gửi kết bạn thành công",friend);    
            }
            catch (Exception ex) {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<Friends>.ErrorResponse("Gửi kết bạn không thành công", new List<string>
                {
                    ex.Message
                });
            }
        }
        public async Task<ApiResponse<Friends>> UpdateFriendStatusAsync(Friends friend)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _friendsRepository.UpdateFriend(friend);
                await _unitOfWork.CommitAsync();
                return ApiResponse<Friends>.SuccessResponse("Cập nhật thành công", friend);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<Friends>.ErrorResponse("Cập nhật không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }
        public async Task<bool> DeleteFriendAsync(int friendId)
        {
            var friend = await _friendsRepository.GetFriendshipAsync(friendId, friendId);
            if (friend == null)
                return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _friendsRepository.DeleteFriend(friend);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetFriendRequest(int friendId)
        {
            try
            {
                var result=await _friendsRepository.GetFriendRequest(friendId);
                return ApiResponse<IEnumerable<FriendRequest>>.SuccessResponse("Danh sách yêu cầu kết bạn", result);
            }
            catch (Exception ex) { 
                return ApiResponse<IEnumerable<FriendRequest>>.ErrorResponse("Danh sách yêu cầu kết bạn", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
