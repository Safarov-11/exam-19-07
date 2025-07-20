using System.Text.Json;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Services;

public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
{
    public async Task SetData<T>(string key, T data, int expiratinonTime)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiratinonTime)
        };

        var JsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        var jsonData = JsonSerializer.Serialize(data, JsonSerializerOptions);
        await cache.SetStringAsync(key, jsonData, options);
    }

    public async Task<T> GetData<T>(string key)
    {
        var jsonData = cache.GetString(key);
        return jsonData == null
        ? default
        : JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task RemoveData(string key)
    {
        await cache.RemoveAsync(key);
    }
}
