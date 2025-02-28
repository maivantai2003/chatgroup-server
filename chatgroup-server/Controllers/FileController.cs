using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chatgroup_server.Dtos;
using chatgroup_server.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        public FileController(IFileService fileService, IConfiguration configuration)
        {
            _fileService = fileService;
            _configuration = configuration;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateFile(FileDto fileDto)
        {
            var file = new Files
            {
                TenFile = fileDto.TenFile,
                DuongDan = fileDto.DuongDan,
                KichThuocFile = fileDto.KichThuocFile,
                LoaiFile = fileDto.LoaiFile,
            };
            var response=await _fileService.CreateFile(file);
            if (response.Success) { 
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var cloudinary = new Cloudinary(new Account(
                _configuration.GetSection("CloudinarySettings:CloudName").Value,
                _configuration.GetSection("CloudinarySettings:ApiKey").Value,
                _configuration.GetSection("CloudinarySettings:ApiSecret").Value
            ));

            var uploadResult = new RawUploadResult();

            // Kiểm tra xem file có phải là ảnh không
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (allowedImageExtensions.Contains(extension))
            {
                // Upload ảnh
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };
                uploadResult = await cloudinary.UploadAsync(imageUploadParams);
            }
            else
            {
                // Upload tệp không phải ảnh
                var rawUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };
                uploadResult = await cloudinary.UploadAsync(rawUploadParams);
            }

            // Kiểm tra lỗi từ Cloudinary
            if (uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message);
            }
            var respone = new FileUpload()
            {
                Url = uploadResult.SecureUrl.ToString(),
                publicId = uploadResult.PublicId
            };
            return Ok(respone);
        }
    }
}
