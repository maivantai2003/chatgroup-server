using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService; 
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetAllConversation(int id)
        {
            var respone=await _conversationService.GetAllConversation(id);
            if (!respone.Success) { 
                return Ok(respone.Errors);
            }
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Console.WriteLine("UserId: "+userId);
            return Ok(respone.Data);    
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateConversation(ConversationDto conversationDto)
        {
            var conversation = new Conversation()
            {
                Avatar = conversationDto.Avatar,
                Content = conversationDto.Content,
                ConversationName = conversationDto.ConversationName,
                UserSend = conversationDto.UserSend,
                Type = conversationDto.Type,
                Id = conversationDto.Id,
                UserId = conversationDto.UserId,
            };
            var respone = await _conversationService.AddConversationAsync(conversation);
            if (!respone.Success)
            {
                return Ok(respone.Errors);
            }
            return Ok(respone.Data);
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetConversationById(int id)
        {
            var respone = await _conversationService.GetConversationByIdAsync(id);
            if (!respone.Success)
            {
                return Ok(respone.Errors);
            }
            return Ok(respone.Data);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateConversation([FromBody]ConversationUpdateDto conversationUpdateDto)
        {
            var conversation = new Conversation()
            {
               UserId=conversationUpdateDto.UserId,
               Id=conversationUpdateDto.Id,
               Type= conversationUpdateDto.Type,    
               UserSend=conversationUpdateDto.UserSend,
               Content= conversationUpdateDto.Content,  
            };
            var respone = await _conversationService.UpdateConversationAsync(conversation);
            if (!respone.Success)
            {
                return Ok(respone.Errors);
            }
            return Ok(respone.Data);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateConversationInfor(ConversationUpdateInfor conversation)
        {
            var result=await _conversationService.UpdateInForConversation(conversation);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return Ok(result.Errors);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateConversationGroup(ConversationUpdateGroupDto conversationUpdateGroupDto)
        {
            var response=await _conversationService.UpdateConversationGroup(conversationUpdateGroupDto);
            if (response.Success) {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }
    }
}
