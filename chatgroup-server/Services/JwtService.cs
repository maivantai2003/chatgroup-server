using chatgroup_server.Common;
using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UAParser;

namespace chatgroup_server.Services
{
    public class JwtService:IJwtService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RedisService _redisServices;
        private static string avatar;
        private static string userName;
        private UserInfor UserInfor;
        public JwtService(ApplicationDbContext context,IConfiguration configuration,IHttpContextAccessor httpContextAccessor, RedisService redisService) {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _redisServices = redisService;
        }

        public async Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string phoneNumber)
        {
            var refreshToken = GenerateRefreshToken();
            var accessToken = GenerateToken(phoneNumber, userId.ToString());
            return await SaveTokenDetails(ipAddress, userId, accessToken, refreshToken);
        }
        private async Task<AuthResponse> SaveTokenDetails(string ipAddress, int userId, string tokenString, string refreshToken)
        {
            var httpContex = _httpContextAccessor.HttpContext;
            var userAgent = httpContex?.Request.Headers["User-Agent"].ToString() ?? "Unknown";
            var parser = Parser.GetDefault();
            UAParser.ClientInfo clientInfo = parser.Parse(userAgent);
            string browserName = clientInfo.UA.Family;
            string browserVersion = clientInfo.UA.Major;
            string osName = clientInfo.OS.Family;
            string deviceName = clientInfo.Device.Family;
            var useRefreshToken = new UserRefreshToken
            {
                CreateDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                IpAddress = ipAddress,
                IsInvalidades = false,
                RefreshToken = refreshToken,
                Token = tokenString,
                UserId = userId
            };
            await _context.UserRefreshTokens.AddAsync(useRefreshToken);
            await _context.SaveChangesAsync();
            return new AuthResponse
            {
                accessToken = tokenString,
                refreshToken = refreshToken,
                IsSuccess = true,
                osName = osName,
                ipAddress = ipAddress,
                Browser = browserName,
            };
        }
        public async Task<AuthResponse> GetTokenAsync(AuthResquest request, string ipAddress)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(x => x.PhoneNumber.Equals(request.PhoneNumber) && x.Password.Equals(request.UserName));
            if (user == null)
            {
                return await Task.FromResult<AuthResponse>(null);
            }
            avatar = user.Avatar;
            userName = user.UserName;
            UserInfor = new UserInfor()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Avatar = user.Avatar,
                Sex = user.Sex,
                Bio = user.Bio,
                Birthday = user.Birthday,
                PhoneNumber=user.PhoneNumber,
                CoverPhoto=user.CoverPhoto
            };
            string tokenString = GenerateToken(user.PhoneNumber, user.UserId.ToString());
            string refreshToken = GenerateRefreshToken();
            return await SaveTokenDetails(ipAddress, user.UserId, tokenString, refreshToken);
        }

        public async Task<bool> IsTokenValid(string accessToken, string ipAddress)
        {
            var isValid = _context.UserRefreshTokens.FirstOrDefault(x => x.Token == accessToken && x.IpAddress == ipAddress) != null;
            return await Task.FromResult(isValid);
        }
        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            using (var crytoProvider = new RNGCryptoServiceProvider())
            {
                crytoProvider.GetBytes(byteArray);
                return Convert.ToBase64String(byteArray);
            }
        }
        private string GenerateToken(string phoneNumber, string Id)
        {
            var jwtKey = _configuration.GetValue<string>("Jwt:key");
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);
            var TokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim("userId",Id),
                    new Claim(ClaimTypes.NameIdentifier,phoneNumber),
                    new Claim("avatar",avatar),
                    new Claim("userName",userName),
                    new Claim("userInfor",JsonConvert.SerializeObject(UserInfor))
                }),
                Expires = DateTime.UtcNow.AddSeconds(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256
                )
            };
            var token = TokenHandler.CreateToken(descriptor);
            string tokenString = TokenHandler.WriteToken(token);
            return tokenString;
        }
        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(request.ExpiredToken);

            var userRefreshToken = _context.UserRefreshTokens.FirstOrDefault(x =>
                x.IsInvalidades == false &&
                x.Token == request.ExpiredToken &&
                x.RefreshToken == request.RefreshToken &&
                x.IpAddress == ipAddress
            );

            var response = ValidateDetails(token, userRefreshToken);
            if (!response.IsSuccess) return response;

            userRefreshToken.IsInvalidades = true;
            _context.UserRefreshTokens.Update(userRefreshToken);
            await _context.SaveChangesAsync();

            var phoneNumber = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return await GetRefreshTokenAsync(ipAddress, userRefreshToken.UserId, phoneNumber);
        }
        private AuthResponse ValidateDetails(JwtSecurityToken token, UserRefreshToken? userRefreshToken)
        {
            if (userRefreshToken == null)
                return new AuthResponse { IsSuccess = false, Reason = "Invalid Token Details." };

            if (token.ValidTo > DateTime.UtcNow)
                return new AuthResponse { IsSuccess = false, Reason = "Token not expired" };

            if (!userRefreshToken.IsActive)
                return new AuthResponse { IsSuccess = false, Reason = "Refresh Token Expired" };

            return new AuthResponse { IsSuccess = true };
        }
    }
}
