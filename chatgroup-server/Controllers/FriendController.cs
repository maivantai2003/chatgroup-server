using chatgroup_server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chatgroup_server.Dtos;
using chatgroup_server.Models;
using chatgroup_server.Interfaces.IServices;
namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;
        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllFriendById(int userId)
        {
            var response=await _friendService.GetFriendsByUserIdAsync(userId);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);   
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddFriend(FriendDto friendDto)
        {
            var friend = new Friends
            {
                FriendId = friendDto.FriendId,
                UserId = friendDto.UserId,
                Status = friendDto.Status,
            };
            var response = await _friendService.AddFriendAsync(friend);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);   
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetFriendRequest(int friendId)
        {
            var response=await _friendService.GetFriendRequest(friendId);
            if (!response.Success) { 
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateFriend(int id,[FromBody]FriendDto friendDto)
        {
            var friend = new Friends()
            {
                Id = id,
                UserId = friendDto.UserId,
                FriendId= friendDto.FriendId,   
                Status = friendDto.Status,
            };
            var response=await _friendService.UpdateFriendStatusAsync(friend);
            if (!response.Success) {
                return Ok(response.Errors);
            }
            return Ok(response.Data);   
        }
        //[HttpPost("[action]")]
        //public async Task<IActionResult> RemoveFriend(int userId, int friendId)
        //{
        //    return Ok();
        //}
        [HttpGet("[action]")]
        public async Task<IActionResult> GetFriendship(int userId, int friendId)
        {
            var response = await _friendService.GetFriendshipAsync(userId, friendId);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}
