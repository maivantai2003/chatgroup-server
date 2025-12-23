using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Repositories
{
    public class UserDeviceRepository : IUserDeviceRepository
    {
        private readonly ApplicationDbContext _context;
        public UserDeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserDevice(UserDevice userDevice)
        {
            await _context.UserDevices.AddAsync(userDevice);
        }

        public Task<List<string>> GetFcmTokensByUserIdAsync(int userId)
        {
            var response = _context.UserDevices
                .AsNoTracking()
                .Where(ud => ud.UserId == userId && !string.IsNullOrEmpty(ud.DeviceToken))
                .Select(ud => ud.DeviceToken!)
                .ToListAsync();
            return response;
        }

        public async Task<List<string>> GetFcmTokensByUserIdsAsync(List<int> userIds)
        {
            return await _context.UserDevices.AsNoTracking().Where(ud => userIds.Contains(ud.UserId) && !string.IsNullOrEmpty(ud.DeviceToken)).Select(ud => ud.DeviceToken!).ToListAsync();
        }

        public async Task<UserDevice?> GetUserDevice(int UserId, string DeviceId)
        {
            var response = await _context.UserDevices.AsNoTracking().FirstOrDefaultAsync(ud => ud.UserId == UserId && ud.DeviceId == DeviceId);
            return response;
        }
        public async Task<bool> UpdateUserDevice(UpdateUserDeviceDto userDevice, string ipAddress)
        {
            var device = await _context.UserDevices.FirstOrDefaultAsync(x =>
            x.UserId == userDevice.UserId &&
            x.DeviceId == userDevice.DeviceId);

            if (device == null)
                return false;
            device.DeviceToken = userDevice.DeviceToken ?? device.DeviceToken;
            device.DeviceType = userDevice.DeviceType ?? device.DeviceType;
            device.Browser = userDevice.Browser ?? device.Browser;
            device.OS = userDevice.OS ?? device.OS;
            device.DeviceName = userDevice.DeviceName ?? device.DeviceName;
            device.Address = userDevice.Address ?? device.Address;

            device.IsOnline = true;
            device.LastActiveAt = DateTime.UtcNow;
            device.IpAddress = ipAddress;
            return true;
        }
    }
}
