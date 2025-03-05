using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMessageFileController : ControllerBase
    {
        private readonly IUserMessageFileService _fileService;

        public UserMessageFileController(IUserMessageFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("AddFileToMessage")]
        public async Task<IActionResult> AddFileToMessage([FromBody] UserMessageFile file)
        {
            var response = await _fileService.AddFileToMessageAsync(file);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetFilesByMessage/{messageId}")]
        public async Task<IActionResult> GetFilesByMessage(int messageId)
        {
            var response = await _fileService.GetFilesByMessageIdAsync(messageId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
