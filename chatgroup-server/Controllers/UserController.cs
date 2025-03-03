using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) { 
            _userService = userService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUser(int userId)
        {
            var response=await _userService.GetAllUsersAsync(userId);
            if (!response.Success) { 
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserById(string numberPhone)
        {
            var respone = await _userService.GetUserByIdAsync(numberPhone);
            if (!respone.Success)
            {
                return Ok(respone.Errors);
            }
            return Ok(respone.Data);
        }


    }
}
