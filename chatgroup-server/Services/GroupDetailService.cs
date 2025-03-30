using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Common;

namespace chatgroup_server.Services
{
    public class GroupDetailService:IGroupDetailService
    {
        private readonly IGroupDetailRepository _groupDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupDetailService(IGroupDetailRepository groupDetailRepository, IUnitOfWork unitOfWork)
        {
            _groupDetailRepository = groupDetailRepository;
            _unitOfWork = unitOfWork;
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
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<ApiResponse<GroupDetail>> DeleteGroupDetailAsync(int groupDetailId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var groupDetail = await _groupDetailRepository.GetGroupDetailByIdAsync(groupDetailId);
                if (groupDetail == null) return ApiResponse<GroupDetail>.ErrorResponse("Xóa không thành công", new List<string>()
                {
                    "Không tìm thấy đối tượng"
                });
                await _groupDetailRepository.DeleteGroupDetail(groupDetail.GroupDetailId);
                await _unitOfWork.CommitAsync();
                return ApiResponse<GroupDetail>.SuccessResponse("Xóa thành công",groupDetail);
            }
            catch(Exception ex) {
                {
                    await _unitOfWork.RollbackAsync();
                    return ApiResponse<GroupDetail>.ErrorResponse("Xóa không thành công", new List<string>()
                {
                    ex.Message
                });
                }
            }
        }
    }
}
