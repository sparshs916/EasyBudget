namespace EasyBudget.Api.Services.Cache;

public interface IRedisCacheService
{
    string createCacheKey(string prefix, string identifier);
    Task SetCacheKeyAsync<T>(string key, T response, TimeSpan expiration);
    Task<T?> GetCacheKeyAsync<T>(string key);
}