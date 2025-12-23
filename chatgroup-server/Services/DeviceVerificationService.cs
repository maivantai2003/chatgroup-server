using chatgroup_server.Common;
using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Exceptions;
using chatgroup_server.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Services
{
    public class DeviceVerificationService : IDeviceVerificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public DeviceVerificationService(
            ApplicationDbContext context,
            IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> VerifyDeviceAsync(VerifyOtpDto dto, string ipAddress)
        {
            // 1. Validate verify-token
            var tokenResult = _jwtService.ValidateVerifyToken(dto.VerifyToken);
            if (!tokenResult.IsValid)
                throw new BusinessException("Verify token không hợp lệ");

            // 2. Check OTP
            var otp = await _context.OtpVerifications.FirstOrDefaultAsync(x =>
                x.UserId == tokenResult.UserId &&
                x.DeviceId == tokenResult.DeviceId &&
                x.Code == dto.Otp &&
                !x.IsUsed &&
                x.ExpiredAt > DateTime.UtcNow
            );

            if (otp == null)
                throw new BusinessException("OTP không đúng hoặc đã hết hạn");

            otp.IsUsed = true;

            // 3. Verify device
            var device = await _context.UserDevices.FirstOrDefaultAsync(x =>
                x.UserId == tokenResult.UserId &&
                x.DeviceId == tokenResult.DeviceId
            );

            if (device == null)
                throw new BusinessException("Không tìm thấy thiết bị");

            device.IsVerified = true;
            device.VerifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // 4. Login thật sự (reuse logic cũ)
            var user = await _context.Users.FindAsync(tokenResult.UserId);
            if (user == null)
                throw new BusinessException("User không tồn tại");

            return await _jwtService.LoginAfterDeviceVerifiedAsync(tokenResult.UserId, ipAddress);
        }
        
    }
}