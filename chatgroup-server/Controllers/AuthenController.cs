using chatgroup_server.Common;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Services;
using chatgroup_server.Dtos;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly RedisService _redisService;
        public AuthenController(IJwtService jwtService,RedisService redisService,IUserService userService) { 
            _jwtService = jwtService;
            _redisService = redisService;   
            _userService = userService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AuthToken([FromBody] AuthResquest authRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse { IsSuccess = false, Reason = "PhoneNumber and Password must be provider" });
            }
            var authResponse = await _jwtService.GetTokenAsync(authRequest, HttpContext.Connection.RemoteIpAddress.ToString());
            if (authResponse == null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]UserRegister userRegister)
        {
            var user = new User
            {
                UserName = userRegister.UserName,
                PhoneNumber = userRegister.PhoneNumber, 
                Avatar = userRegister.Avatar,   
                Birthday = userRegister.Birthday,
                Sex = userRegister.Sex, 
                Password = userRegister.Password,   
            };
            var response=await _userService.AddUserAsync(user);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }else if (response == null)
            {
                return Ok(new
                {
                    response.Errors
                });
            }
            return Ok(response.Data);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse { IsSuccess = false, Reason = "UserName and Password must be provider" });
            }

            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var response = await _jwtService.RefreshTokenAsync(request, ipAddress);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
