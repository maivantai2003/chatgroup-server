using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class GroupDetailService:IGroupDetailService
    {
        private readonly IGroupDetailRepository _groupDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly RedisService _redisService;

        public GroupDetailService(IGroupDetailRepository groupDetailRepository, IUnitOfWork unitOfWork, IUserContextService userContextService, RedisService redisService)
        {
            _groupDetailRepository = groupDetailRepository;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _redisService = redisService;
        }

        public async Task<GroupDetail?> GetGroupDetailByIdAsync(int groupDetailId)
        {
            return await _groupDetailRepository.GetGroupDetailByIdAsync(groupDetailId);
        }

        public async Task<IEnumerable<GroupDetail>> GetGroupDetailsByGroupIdAsync(int groupId)
        {
            return await _groupDetailRepository.GetGroupDetailsByGroupIdAsync(groupId);
        }

        public async Task<IEnumerable<GroupDetail>> GetGroupDetailsByUserIdAsync(int userId)
        {
            return await _groupDetailRepository.GetGroupDetailsByUserIdAsync(userId);
        }

        public async Task<ApiResponse<GroupDetail>> AddGroupDetailAsync(GroupDetail groupDetail)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _groupDetailRepository.AddGroupDetailAsync(groupDetail);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.GroupsOfUser(groupDetail.UserId));
                return ApiResponse<GroupDetail>.SuccessResponse("Thêm thành viên thành công", groupDetail);
                
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<GroupDetail>.ErrorResponse("Thêm thành viên không thành công", new List<string>()
                {
                    ex.Message
                });
            }
        }

        public async Task<bool> UpdateGroupDetailAsync(GroupDetail groupDetail)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _groupDetailRepository.UpdateGroupDetail(groupDetail);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.GroupsOfUser(groupDetail.UserId));
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<ApiResponse<LeaveGroupDetailDto>> DeleteGroupDetailAsync(LeaveGroupDetailDto groupDetailDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //var groupDetail = await _groupDetailRepository.GetGroupDetailByIdAsync(groupDetailId);
                //if (groupDetail == null) return ApiResponse<GroupDetail>.ErrorResponse("Xóa không thành công", new List<string>()
                //{
                //    "Không tìm thấy đối tượng"
                //});
                await _groupDetailRepository.DeleteGroupDetail(groupDetailDto);
                await _unitOfWork.CommitAsync();
                await _redisService.RemoveCacheAsync(CacheKeys.GroupsOfUser(groupDetailDto.userId));
                return ApiResponse<LeaveGroupDetailDto>.SuccessResponse("Xóa thành công",groupDetailDto);
            }
            catch(Exception ex) {
                {
                    await _unitOfWork.RollbackAsync();
                    return ApiResponse<LeaveGroupDetailDto>.ErrorResponse("Xóa không thành công", new List<string>()
                {
                    ex.Message
                });
                }
            }
        }
    }
}
