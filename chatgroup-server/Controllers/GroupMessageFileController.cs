using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chatgroup_server.Models;
namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupMessageFileController : ControllerBase
    {
        private readonly IGroupMessageFileService _groupMessageFileService;
        public GroupMessageFileController(IGroupMessageFileService groupMessageFileService)
        {
            _groupMessageFileService = groupMessageFileService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllGroupMessageFile(int groupId)
        {
            var response = await _groupMessageFileService.GetAllFileGroupMessage(groupId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody]GroupMessageFileAddDto groupMessageFileDto)
        {
            var groupMessageFile = new GroupMessageFile()
            {
                FileId=groupMessageFileDto.FileId,
                GroupedMessageId=groupMessageFileDto.GroupedMessageId,
            };
            var response=await _groupMessageFileService.AddAsync(groupMessageFile);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return Ok(response.Errors);
        }
    }
}
