using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudMessageController : ControllerBase
    {
        private readonly ICloudMessageService _messageService;

        public CloudMessageController(ICloudMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetMessagesByUserId(int userId)
        {
            var response = await _messageService.GetMessagesByUserIdAsync(userId);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var response = await _messageService.GetMessageByIdAsync(id);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddMessage([FromBody] CloudMessage message)
        {
            var response = await _messageService.AddMessageAsync(message);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] CloudMessage message)
        {
            message.CloudMessageId = id;
            var response = await _messageService.UpdateMessageAsync(message);
            return response.Success ? Ok(response.Data) : BadRequest(response.Errors);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var success = await _messageService.DeleteMessageAsync(id);
            return success ? Ok("Xóa tin nhắn thành công") : BadRequest("Xóa tin nhắn thất bại");
        }
    }
}
