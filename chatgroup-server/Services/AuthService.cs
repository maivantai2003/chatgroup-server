using chatgroup_server.Common;
using chatgroup_server.Data;
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace chatgroup_server.Services
{
    public class AuthService : IAuthService
    {
        //private readonly ApplicationDbContext _context;
        //private readonly IJwtService _jwtService;
        //private readonly IRedisService _redis;
        ////private readonly IEmailService _email;
        //public AuthService(ApplicationDbContext context, IJwtService jwtService, IRedisService redis)
        //{
        //    _context = context;
        //    _jwtService = jwtService;
        //    _redis = redis;
        //}

        //public async Task<AuthResponse> LoginAsync(AuthResquest req, string ip)
        //{
        //    var user = await _context.Users
        //    .FirstOrDefaultAsync(x => x.PhoneNumber == req.PhoneNumber);

        //    if (user == null || !PasswordHelper.Verify(req.UserName, user.Password))
        //    {
        //        return new AuthResponse
        //        {
        //            IsSuccess = false,
        //            Reason = "Sai thông tin đăng nhập"
        //        };
        //    }
        //    var device = await _context.UserDevices.FirstOrDefaultAsync(x =>
        //    x.UserId == user.UserId &&
        //    x.DeviceId == req.DeviceId);
        //    if (device != null && device.IsVerified)
        //    {
        //        device.LastLoginAt = DateTime.UtcNow;
        //        await _context.SaveChangesAsync();

        //        return await IssueTokensAsync(user, ip);
        //    }
        //    var verifyToken = _jwtService.GenerateVerifyDeviceToken(
        //    user.UserId,
        //    req.DeviceId
        //);

        //    await _redis.SetCacheAsync<string>(
        //        $"verify_device:{verifyToken}",
        //        user.UserId.ToString(),
        //        TimeSpan.FromMinutes(5)
        //    );

        //    await _email.SendVerifyDeviceMail(
        //        user.Gmail,
        //        verifyToken,
        //        req.DeviceName,
        //        req.Browser,
        //        req.OS
        //    );

        //    return new AuthResponse
        //    {
        //        IsSuccess = false,
        //        RequireDeviceVerification = true,
        //        VerifyToken = verifyToken
        //    };
        //}

        //public async Task<AuthResponse> VerifyDeviceAsync(string verifyToken, string ip)
        //{
        //    var verifyResult = _jwtService.ValidateVerifyDeviceToken(verifyToken);
        //    if (!verifyResult.IsValid)
        //    {
        //        return new AuthResponse
        //        {
        //            IsSuccess = false,
        //            Reason = "Verify token không hợp lệ"
        //        };
        //    }

        //    var cacheKey = $"verify_device:{verifyToken}";
        //    var cached = await _redis.GetCacheAsync<string>(cacheKey);

        //    if (cached == null)
        //    {
        //        return new AuthResponse
        //        {
        //            IsSuccess = false,
        //            Reason = "OTP đã hết hạn"
        //        };
        //    }

        //    var device = await _context.UserDevices.FirstOrDefaultAsync(x =>
        //        x.UserId == verifyResult.UserId &&
        //        x.DeviceId == verifyResult.DeviceId);

        //    if (device == null)
        //    {
        //        device = new UserDevice
        //        {
        //            UserId = verifyResult.UserId,
        //            DeviceId = verifyResult.DeviceId,
        //            IsVerified = true,
        //            VerifiedAt = DateTime.UtcNow,
        //            LastLoginAt = DateTime.UtcNow
        //        };
        //        _context.UserDevices.Add(device);
        //    }
        //    else
        //    {
        //        device.IsVerified = true;
        //        device.VerifiedAt = DateTime.UtcNow;
        //        device.LastLoginAt = DateTime.UtcNow;
        //    }

        //    await _context.SaveChangesAsync();
        //    await _redis.RemoveCacheAsync(cacheKey);

        //    var user = await _context.Users.FindAsync(verifyResult.UserId);
        //    return await IssueTokensAsync(user!, ip);
        //}
        //private async Task<AuthResponse> IssueTokensAsync(User user, string ip)
        //{
        //    var accessToken = _jwtService.GenerateAccessToken(user);
        //    var refreshToken = _jwtService.GenerateRefreshToken();

        //    var refreshEntity = new UserRefreshToken
        //    {
        //        UserId = user.UserId,
        //        Token = accessToken,
        //        RefreshToken = refreshToken,
        //        IpAddress = ip,
        //        CreateDate = DateTime.UtcNow,
        //        ExpirationDate = DateTime.UtcNow.AddMinutes(45),
        //        IsInvalidades = false
        //    };

        //    _context.UserRefreshTokens.Add(refreshEntity);
        //    await _context.SaveChangesAsync();

        //    return new AuthResponse
        //    {
        //        IsSuccess = true,
        //        accessToken = accessToken,
        //        refreshToken = refreshToken
        //    };
        //}
    }
}
