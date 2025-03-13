using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chatgroup_server.Models;
using chatgroup_server.Services;
using MimeKit;
namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupMessageController : ControllerBase
    {
        private readonly IGroupMessageService _groupMessageService;
        public GroupMessageController(IGroupMessageService groupMessageService)
        {
            _groupMessageService = groupMessageService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddGroupMessage(GroupMessageDto groupMessageDto)
        {
            var groupMessage = new GroupMessages()
            {
                SenderId = groupMessageDto.SenderId,
                GroupId = groupMessageDto.GroupId,
                ReplyToMessageId = groupMessageDto.ReplyToMessageId,
                MessageType = groupMessageDto.MessageType,
                Content = groupMessageDto.Content,
            };
            var response=await _groupMessageService.AddGroupMessage(groupMessage);
            if (response.Success) {
                return Ok(response.Data);
            }
            return BadRequest(response.Data);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllGroupMessage(int id)
        {
            var response = await _groupMessageService.GetAllGroupMessageById(id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }
    }
}
