namespace chatgroup_server.Interfaces.IServices
{
    public interface IRedisService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<T> GetCacheAsync<T>(string key);
        Task RemoveCacheAsync(string key);
    }
}
