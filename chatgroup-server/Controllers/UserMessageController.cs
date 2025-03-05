using chatgroup_server.Dtos;
using chatgroup_server.Models;
using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMessageController : ControllerBase
    {
        private readonly IUserMessageService _userMessageService;
        public UserMessageController(IUserMessageService userMessageService) {
            _userMessageService = userMessageService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUserMessage(SendUserMessageDto userMessageDto)
        {
            var UserMessage = new UserMessages()
            {
                Content = userMessageDto.Content,
                SenderId = userMessageDto.SenderId, 
                ReceiverId = userMessageDto.ReceiverId,
                MessageType = userMessageDto.MessageType,   
            };
            var response=await _userMessageService.AddUserMessageAsync(UserMessage);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}
