using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IRepositories;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace chatgroup_server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(int userId)
        {
            //var usersWithFriendStatus = _context.Friends
            //    .Where(f => f.Status == 0 || f.Status == 1)
            //    .Select(f => f.UserId)
            //    .Union(_context.Friends.Where(f => f.Status == 0 || f.Status == 1).Select(f => f.FriendId));

            //return await _context.Users
            //    .AsNoTracking()
            //    .Where(u => u.Status == 1 && u.UserId != userId && !usersWithFriendStatus.Contains(u.UserId))
            //    .Select(u => new UserDto
            //    {
            //        UserId = u.UserId,
            //        UserName = u.UserName,
            //        Avatar = u.Avatar
            //    })
            //    .ToListAsync();
            var friendIds = await _context.Friends
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && (f.Status == 0 || f.Status == 1))
                    .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                    .ToListAsync();

            // Lấy danh sách user chưa kết bạn
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Status == 1 && u.UserId != userId && !friendIds.Contains(u.UserId))
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Avatar = u.Avatar
                })
                .ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(string numberPhone)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.PhoneNumber.Equals(numberPhone));
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<UserInfor?> GetUserById(int userId)
        {
            var result=await _context.Users.FindAsync(userId);
            return new UserInfor()
            {
                UserId=userId,
                UserName=result.UserName,
                Bio=result.Bio,
                Avatar=result.Avatar,
                Birthday=result.Birthday,
                CoverPhoto=result.CoverPhoto,
                PhoneNumber = result.PhoneNumber,
                Sex = result.Sex,
                Password=result.Password
            };
        }
        public async Task<bool> CheckPhoneNumber(string ?phoneNumber)
        {
           var result= await _context.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.PhoneNumber==phoneNumber); 
           return result!=null?true:false;  
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var result =await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Gmail == email);
            return result;
        }

        public async Task UpdateUserByEmail(string password, string email)
        {
            //await _context.Users.Where(x => x.Gmail == email)
            //    .ExecuteUpdateAsync(u => u.SetProperty(x => x.Password, password));
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Gmail == email);
            if (user != null)
            {
                //user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                user.Password= password;
                _context.Users.Update(user);
            }
        }
    }
}
