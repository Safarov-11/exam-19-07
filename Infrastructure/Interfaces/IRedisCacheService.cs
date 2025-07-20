namespace Infrastructure.Interfaces;

public interface IRedisCacheService
{
    Task SetData<T>(string key, T data, int expiratinonTime);
    Task<T> GetData<T>(string key);
    Task RemoveData(string key);
}
