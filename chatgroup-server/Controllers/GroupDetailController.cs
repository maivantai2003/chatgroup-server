using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupDetailController : ControllerBase
    {
        private readonly IGroupDetailService _groupDetailService;
        public GroupDetailController(IGroupDetailService groupDetailService) { 
            _groupDetailService = groupDetailService;   
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateGroupDetail(GroupDetailDto groupDetailDto)
        {
            var groupDetail = new GroupDetail()
            {
                UserId = groupDetailDto.UserId,
                GroupId = groupDetailDto.GroupId,
                Role = groupDetailDto.Role,
            };
            var response = await _groupDetailService.AddGroupDetailAsync(groupDetail);
            if (!response.Success) { 
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}
