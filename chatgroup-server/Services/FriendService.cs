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

        public async Task<IEnumerable<Friends>> GetFriendsByUserIdAsync(int userId)
        {
            return await _friendsRepository.GetFriendsByUserIdAsync(userId);
        }
        public async Task<bool> AddFriendAsync(Friends friend)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _friendsRepository.AddFriendAsync(friend);
                await _unitOfWork.CommitAsync();
                return true;    
            }
            catch (Exception ex) {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
        public async Task<bool> UpdateFriendStatusAsync(Friends friend)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _friendsRepository.UpdateFriend(friend);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
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
    }
}
