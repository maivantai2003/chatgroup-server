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
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetGroupById(int id) { 
            var response=await _groupService.GetGroupByIdAsync(id);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateGroup(GroupUpdateDto groupUpdateDto)
        {
            var group = new Group()
            {
                GroupId = groupUpdateDto.GroupId,
                GroupName = groupUpdateDto.GroupName,
                Avatar = groupUpdateDto.Avatar,
                Status = groupUpdateDto.Status,
            };
            var response = await _groupService.UpdateGroupAsync(group);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }

    }
}
