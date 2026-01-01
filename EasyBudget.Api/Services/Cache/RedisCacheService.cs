namespace EasyBudget.Api.Services.Cache;

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache? _cache;
    private readonly ILogger<RedisCacheService> _logger;
    public RedisCacheService(IDistributedCache cache,
           ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }
    public string createCacheKey(string prefix, string identifier)
    {
        return $"{prefix}_{identifier}";
    }
    public async Task SetCacheKeyAsync<T>(string key, T response,
        TimeSpan expiration)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var json = JsonSerializer.Serialize(response);

            await _cache.SetStringAsync(key, json, options);

            _logger.LogInformation("Successfully cached key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache idempotency key: {Key}", key);
            throw;
        }
    }

    public async Task<T?> GetCacheKeyAsync<T>(string key)
    {
        try
        {
            var cachedValue = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedValue))
            {
                _logger.LogDebug("Cache key not found: {Key}", key);
                return default;
            }

            _logger.LogInformation("Cache key found: {Key}", key);
            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cache key: {Key}", key);
            throw; // Re-throw so callers know Redis failed
        }
    }

    public async Task RemoveCacheKeyAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            _logger.LogInformation("Successfully removed cache key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove cache key: {Key}", key);
            throw;
        }
    }
}
