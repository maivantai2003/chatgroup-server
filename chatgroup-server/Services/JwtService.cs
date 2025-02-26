using chatgroup_server.Common;
using chatgroup_server.Data;
using chatgroup_server.Interfaces.IServices;

namespace chatgroup_server.Services
{
    public class JwtService:IJwtService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly RedisService _redisService;
        public JwtService(ApplicationDbContext context,IConfiguration configuration,IHttpContextAccessor contextAccessor,RedisService redisService) {
            _context = context;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _redisService = redisService;
        }

        public Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> GetTokenAsync(AuthResquest request, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsTokenValid(string accessToken, string ipAddress)
        {
            throw new NotImplementedException();
        }
    }
}
