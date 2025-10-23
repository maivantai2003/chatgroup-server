using chatgroup_server.Interfaces.IServices;

namespace chatgroup_server.Services
{
    public class OtpService:IOtpService
    {
        private readonly IRedisService _redisService;
        private static readonly Random _random = new Random();
        public OtpService(IRedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task<string> GenerateOtpAsync(string key, int length = 6, int expiryInMinutes = 5)
        {
            var otp=_random.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length)).ToString();
            var result=await _redisService.GetCacheAsync<string>(key);
            if (result!=null)
            {
                await _redisService.RemoveCacheAsync(key);
            }
            await _redisService.SetCacheAsync<string>(key, otp,TimeSpan.FromMinutes(expiryInMinutes));
            return otp;
        }
        public async Task<bool> ValidateOtpAsync(string key, string otp)
        {
            var storedOtp =await _redisService.GetCacheAsync<string>(key);
            if(storedOtp==null)
            {
                return false;
            }
            if (storedOtp ==otp)
            {
                await _redisService.RemoveCacheAsync(key);
                return true;
            }
            return false;
        }
    }
}
