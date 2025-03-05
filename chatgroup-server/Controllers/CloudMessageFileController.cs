using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudMessageFileController : ControllerBase
    {
        private readonly ICloudMessageFileService _service;

        public CloudMessageFileController(ICloudMessageFileService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            if(response.Success) {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] CloudMessageFile cloudMessageFile)
        {
            var response = await _service.AddAsync(cloudMessageFile);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CloudMessageFile cloudMessageFile)
        {
            if (id != cloudMessageFile.CloudMessageFileId)
                return BadRequest("ID không hợp lệ");

            var response = await _service.UpdateAsync(cloudMessageFile);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound("Không tìm thấy dữ liệu");

            return Ok("Xóa thành công");
        }
    }
}
