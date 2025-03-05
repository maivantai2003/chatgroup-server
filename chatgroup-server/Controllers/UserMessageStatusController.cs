using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMessageStatusController : ControllerBase
    {
        private readonly IUserMessageStatusService _service;

        public UserMessageStatusController(IUserMessageStatusService service)
        {
            _service = service;
        }

        [HttpGet("GetByReceiver/{receiverId}")]
        public async Task<IActionResult> GetUserMessageStatuses(int receiverId)
        {
            var response = await _service.GetUserMessageStatusesAsync(receiverId);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetUserMessageStatus(int id)
        {
            var response = await _service.GetUserMessageStatusByIdAsync(id);
            return response.Success ? Ok(response.Data) : NotFound(response.Errors);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserMessageStatus([FromBody] UserMessageStatus status)
        {
            var response = await _service.AddUserMessageStatusAsync(status);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUserMessageStatus(int id, [FromBody] UserMessageStatus status)
        {
            if (id != status.UserMessageStatusId) return BadRequest("ID không khớp");
            var response = await _service.UpdateUserMessageStatusAsync(status);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUserMessageStatus(int id)
        {
            var response = await _service.DeleteUserMessageStatusAsync(id);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }
    }
}
