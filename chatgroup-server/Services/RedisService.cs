using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace chatgroup_server.Services
{
    public class RedisService
    {
        private readonly IDistributedCache _cache;
        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            var serializedValue = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedValue, options);
        }
        public async Task<T> GetCacheAsync<T>(string key)
        {
            var cacheValue = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cacheValue))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(cacheValue);
        }
        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
