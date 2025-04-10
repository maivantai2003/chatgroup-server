using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

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
        public async Task<IActionResult> CheckPhoneNumber(string? phoneNumber)
        {
            var response = await _userService.CheckPhoneNumber(phoneNumber);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);
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
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var response=await _userService.GetUserById(userId);
            if (!response.Success) {
                return Ok(response.Errors);
            }
            return Ok(response.Data);   
        }
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateUser(int id,[FromBody]UserUpdateDto userUpdateDto)
        {
            var user=new User()
            {
                UserId = userUpdateDto.UserId,
                UserName = userUpdateDto.UserName,
                Avatar=userUpdateDto.Avatar,
                Bio=userUpdateDto.Bio,
                CoverPhoto=userUpdateDto.CoverPhoto,
                Status=userUpdateDto.Status,
                Birthday=userUpdateDto.Birthday,
                PhoneNumber=userUpdateDto.PhoneNumber,
                Sex=userUpdateDto.Sex,  
            };
            var response = await _userService.UpdateUserAsync(user);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }

    }
}
