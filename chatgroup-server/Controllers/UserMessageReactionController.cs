using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMessageReactionController : ControllerBase
    {
        private readonly IUserMessageReactionService _userMessageReactionService;
        public UserMessageReactionController(IUserMessageReactionService userMessageReactionService)
        {
            _userMessageReactionService = userMessageReactionService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddReaction(UserMessageReaction userMessageReaction)
        {
            var response = await _userMessageReactionService.AddReactionAsync(userMessageReaction);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("[action]/{messageId}")]
        public async Task<IActionResult> GetReactionsByMessageId(int messageId)
        {
            var response = await _userMessageReactionService.GetReactionsByMessageIdAsync(messageId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
