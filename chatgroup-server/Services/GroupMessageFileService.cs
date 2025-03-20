using chatgroup_server.Common;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Interfaces;
using chatgroup_server.Models;
using chatgroup_server.Dtos;
using chatgroup_server.Repositories;
using MimeKit;

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

        public async Task<ApiResponse<GroupMessageFileResponseDto>> AddAsync(GroupMessageFile groupMessageFile)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _groupMessageFileRepository.AddAsync(groupMessageFile);
                await _unitOfWork.CommitAsync();
                var result = await _groupMessageFileRepository.GetGroupMessageFile(groupMessageFile.GroupMessageFileId);
                return ApiResponse<GroupMessageFileResponseDto>.SuccessResponse("Thêm mới thành công",result);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<GroupMessageFileResponseDto>.ErrorResponse("Thêm mới thất bại", new List<string> { ex.Message });
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

        public async Task<ApiResponse<IEnumerable<GroupMessageFileResponseDto>>> GetAllFileGroupMessage(int groupId)
        {
            try
            {
                var result = await _groupMessageFileRepository.GetAllFileGroupMessage(groupId);
                return ApiResponse<IEnumerable<GroupMessageFileResponseDto>>.SuccessResponse("Danh sách file group", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GroupMessageFileResponseDto>>.ErrorResponse("Lỗi khi lấy danh sách file group", new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
