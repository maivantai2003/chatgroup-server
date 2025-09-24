using chatgroup_server.Common;
using chatgroup_server.Data;
using chatgroup_server.Dtos;
using chatgroup_server.Helpers;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
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
        private readonly IRedisService _redisServices;
        private static string avatar;
        private static string userName;
        private UserInfor UserInfor;
        public JwtService(ApplicationDbContext context,IConfiguration configuration,IHttpContextAccessor httpContextAccessor, IRedisService redisService) {
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
                //ExpirationDate = DateTime.UtcNow.AddDays(7),
                ExpirationDate = DateTime.UtcNow.AddMinutes(45),
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
        private async Task<AuthResponse> CreateAuthResponseAsync(User user, string ipAddress)
        {
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
                PhoneNumber = user.PhoneNumber,
                CoverPhoto = user.CoverPhoto
            };

            string tokenString = GenerateToken(user.PhoneNumber ?? user.Gmail, user.UserId.ToString());
            string refreshToken = GenerateRefreshToken();
            return await SaveTokenDetails(ipAddress, user.UserId, tokenString, refreshToken);
        }
        public async Task<AuthResponse> GetTokenAsync(AuthResquest request, string ipAddress)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(x => x.PhoneNumber.Equals(request.PhoneNumber));
            if (user == null)
            {
                return await Task.FromResult<AuthResponse>(null);
            }
            if(!PasswordHelper.Verify(request.UserName, user.Password))
            {
                return null;
            }
            return await CreateAuthResponseAsync(user, ipAddress);
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
                    new Claim(ClaimTypes.NameIdentifier,Id),
                    new Claim("avatar",avatar),
                    new Claim("userName",userName),
                    new Claim("userInfor",JsonConvert.SerializeObject(UserInfor))
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
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
        public string GenerateResetPasswordToken(string email)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("purpose", "reset_password")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public (bool IsValid, string? Email, string? Reason) DecodeResetPasswordToken(string token)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero 
                }, out SecurityToken validatedToken);

                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var purpose = principal.FindFirst("purpose")?.Value;

                if (purpose != "reset_password")
                {
                    return (false, null, "Invalid token purpose.");
                }

                return (true, email, null);
            }
            catch (SecurityTokenExpiredException)
            {
                return (false, null, "Token expired.");
            }
            catch (Exception ex)
            {
                return (false, null, "Invalid token: " + ex.Message);
            }
        }
        public async Task<AuthResponse> GoogleLogin(string token, string ipAddress)
        {
            try
            {
                //Console.WriteLine(token);
                var payload=await GoogleJsonWebSignature.ValidateAsync(token);
                //Console.WriteLine(payload);
                var email=payload.Email;
                var name=payload.Name;
                var picture=payload.Picture;
                var user=await _context.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.Gmail==email);
                Console.WriteLine(email);
                if (user != null) {
                    return await CreateAuthResponseAsync(user, ipAddress);
                }
                return new AuthResponse()
                {
                    IsSuccess = false,
                    Reason = "Người dùng chưa được tạo"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Reason = "Token Google không hợp lệ: " + ex.Message
                };
            }
        }

    }
}
