using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDeviceController : ControllerBase
    {
        private readonly IUserDeviceService _userDeviceService;
        public UserDeviceController(IUserDeviceService userDeviceService)
        {
            _userDeviceService = userDeviceService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUserDevice(UserDeviceAddDto userDeviceAddDto)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _userDeviceService.AddUserDevice(userDeviceAddDto, ipAddress);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Errors);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserDevice(int userId, string deviceId)
        {
            var response = await _userDeviceService.GetUserDevice(userId, deviceId);
            if (response.Success)
            {
                return Ok(response.Data == null ? null : response.Data);
            }
            return BadRequest(response.Errors);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUserDevice(UpdateUserDeviceDto userDeviceUpdateDto)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _userDeviceService.UpdateUserDeviceAsync(userDeviceUpdateDto, ipAddress);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Errors);
        }
    }
}
