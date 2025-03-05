using chatgroup_server.Common;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Interfaces;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class GroupMessageFileService : IGroupMessageFileService
    {
        private readonly IGroupMessageFileRepository _groupMessageFileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupMessageFileService(IGroupMessageFileRepository groupMessageFileRepository, IUnitOfWork unitOfWork)
        {
            _groupMessageFileRepository = groupMessageFileRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<GroupMessageFile>>> GetAllAsync()
        {
            var result = await _groupMessageFileRepository.GetAllAsync();
            return ApiResponse<IEnumerable<GroupMessageFile>>.SuccessResponse("Danh sách GroupMessageFile", result);
        }

        public async Task<ApiResponse<GroupMessageFile>> GetByIdAsync(int id)
        {
            var result = await _groupMessageFileRepository.GetByIdAsync(id);
            if (result == null)
            {
                return ApiResponse<GroupMessageFile>.ErrorResponse("Không tìm thấy dữ liệu", new List<string> { "Không có dữ liệu" });
            }
            return ApiResponse<GroupMessageFile>.SuccessResponse("Chi tiết GroupMessageFile", result);
        }

        public async Task<ApiResponse<GroupMessageFile>> AddAsync(GroupMessageFile groupMessageFile)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _groupMessageFileRepository.AddAsync(groupMessageFile);
                await _unitOfWork.CommitAsync();
                return ApiResponse<GroupMessageFile>.SuccessResponse("Thêm mới thành công", groupMessageFile);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<GroupMessageFile>.ErrorResponse("Thêm mới thất bại", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<GroupMessageFile>> UpdateAsync(GroupMessageFile groupMessageFile)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _groupMessageFileRepository.Update(groupMessageFile);
                await _unitOfWork.CommitAsync();
                return ApiResponse<GroupMessageFile>.SuccessResponse("Cập nhật thành công", groupMessageFile);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<GroupMessageFile>.ErrorResponse("Cập nhật thất bại", new List<string> { ex.Message });
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _groupMessageFileRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _groupMessageFileRepository.Delete(entity);
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
