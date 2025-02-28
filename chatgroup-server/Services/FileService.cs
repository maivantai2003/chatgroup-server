using chatgroup_server.Common;
using chatgroup_server.Interfaces;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;

namespace chatgroup_server.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        public FileService(IFileRepository fileRepository, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<Files>> CreateFile(Files file)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _fileRepository.CreateFileAsync(file);
                await _unitOfWork.CommitAsync();
                return ApiResponse<Files>.SuccessResponse("Thêm file thành công",file);
            }
            catch (Exception ex) { 
                await _unitOfWork.RollbackAsync();
                return ApiResponse<Files>.ErrorResponse("Thêm file không thành công",new List<string>()
                {
                    ex.Message
                });
            }
        }
    }
}
