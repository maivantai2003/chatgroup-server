using chatgroup_server.Data;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;

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
    }
}
