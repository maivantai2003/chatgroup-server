using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chatgroup_server.Dtos;
using System.Net.WebSockets;
using chatgroup_server.Models;
namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService) { 
            _groupService = groupService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateGroup(GroupDto groupDto)
        {
            var group = new Group()
            {
                Avatar = groupDto.Avatar,
                GroupName = groupDto.GroupName,
            };
            var respone=await _groupService.AddGroupAsync(group);
            if (!respone.Success)
            {
                return Ok(respone.Errors);
            }
            return Ok(respone.Data);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllGroupById(int userId)
        {
            var response=await _groupService.GetAllGroupsAsync(userId);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}
