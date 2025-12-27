using chatgroup_server.Common;
using chatgroup_server.Dtos;
using chatgroup_server.Exceptions;
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace chatgroup_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IRedisService _redisService;
        private readonly IRecaptchaService _recaptchaService;
        private readonly IUserContextService _userContextService;
        private readonly IDeviceVerificationService _deviceVerificationService;
        public AuthenController(IJwtService jwtService, IRedisService redisService, IUserService userService, IRecaptchaService recaptchaService, IUserContextService userContextService, IDeviceVerificationService deviceVerificationService)
        {
            _jwtService = jwtService;
            _redisService = redisService;
            _userService = userService;
            _recaptchaService = recaptchaService;
            _userContextService = userContextService;
            _deviceVerificationService = deviceVerificationService;
        }
        [EnableRateLimiting("auth-login")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] AuthResquest authRequest)
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
            Console.WriteLine(_userContextService.GetCurrentUserId());
            return Ok(authResponse);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            var user = new User
            {
                UserName = userRegister.UserName,
                PhoneNumber = userRegister.PhoneNumber,
                Avatar = userRegister.Avatar,
                Birthday = userRegister.Birthday,
                Sex = userRegister.Sex,
                Password = PasswordHelper.Hash(userRegister.Password),
                Gmail = userRegister.Gmail
            };
            var response = await _userService.AddUserAsync(user);
            if (!response.Success)
            {
                return Ok(response.Errors);
            }
            else if (response == null)
            {
                return Ok(new
                {
                    response.Errors
                });
            }
            return Ok(response.Data);
        }
        [HttpPost("verify-device")]
        public async Task<IActionResult> VerifyDevice([FromBody] VerifyOtpDto dto)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress!.ToString();
                var result = await _deviceVerificationService.VerifyDeviceAsync(dto, ip);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var isHuman = await _recaptchaService.Verify(request.CaptchaToken);
            if (!isHuman)
            {
                return BadRequest("Captcha không hợp lệ");
            }
            var response = await _userService.ForgotPassword(request);
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Message);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response = await _userService.ResetPassword(request);
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleAuthen google)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse { IsSuccess = false, Reason = "PhoneNumber and Password must be provider" });
            }
            var authResponse = await _jwtService.GoogleLogin(google.Token, HttpContext.Connection.RemoteIpAddress.ToString());
            if (authResponse == null)
            {
                return Unauthorized();
            }
            Console.WriteLine("userId: " + _userContextService.GetCurrentUserId());
            return Ok(authResponse);
        }
    }
}