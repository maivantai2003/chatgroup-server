using chatgroup_server.Common;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Interfaces;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class CloudMessageFileService : ICloudMessageFileService
    {
        private readonly ICloudMessageFileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CloudMessageFileService(ICloudMessageFileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<CloudMessageFile>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return ApiResponse<IEnumerable<CloudMessageFile>>.SuccessResponse("Danh sách CloudMessageFile", result);
        }

        public async Task<ApiResponse<CloudMessageFile>> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
                return ApiResponse<CloudMessageFile>.ErrorResponse("Không tìm thấy dữ liệu");

            return ApiResponse<CloudMessageFile>.SuccessResponse("Chi tiết CloudMessageFile", result);
        }

        public async Task<ApiResponse<CloudMessageFile>> AddAsync(CloudMessageFile cloudMessageFile)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.AddAsync(cloudMessageFile);
                await _unitOfWork.CommitAsync();
                return ApiResponse<CloudMessageFile>.SuccessResponse("Thêm thành công", cloudMessageFile);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return ApiResponse<CloudMessageFile>.ErrorResponse("Thêm thất bại");
            }
        }

        public async Task<ApiResponse<CloudMessageFile>> UpdateAsync(CloudMessageFile cloudMessageFile)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _repository.Update(cloudMessageFile);
                await _unitOfWork.CommitAsync();
                return ApiResponse<CloudMessageFile>.SuccessResponse("Cập nhật thành công", cloudMessageFile);
            }
            catch(Exception ex) {
                {
                    await _unitOfWork.RollbackAsync();
                    return ApiResponse<CloudMessageFile>.ErrorResponse("Cập nhật thất bại", new List<string>()
                {
                    ex.Message
                });
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cloudMessageFile = await _repository.GetByIdAsync(id);
            if (cloudMessageFile == null)
                return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _repository.Delete(cloudMessageFile);
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
