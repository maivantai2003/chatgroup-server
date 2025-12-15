using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
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

        public async Task<UserDevice?> GetUserDevice(int UserId, string DeviceId)
        {
            var response = await _context.UserDevices.AsNoTracking().FirstOrDefaultAsync(ud => ud.UserId == UserId && ud.DeviceId == DeviceId);
            return response;
        }
        public Task UpdateUserDevice(UserDeviceUpdateDto userDevice)
        {
            throw new NotImplementedException();
        }
    }
}
