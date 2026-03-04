namespace api.Data;

using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

public class CacheContext
{
    private readonly IDatabase db;

    public CacheContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        var muxer = ConnectionMultiplexer.Connect(connectionString);
        db = muxer.GetDatabase();
    }

    public string? GetCache(string key)
    {
        return db.StringGet(key);
    }

    public void NewCache(string key, string value, TimeSpan? expiration = null)
    {
        if (expiration.HasValue)
        {
            _ = db.StringSet(key, value, (Expiration)(TimeSpan)expiration);
        }
        else
        {
            _ = db.StringSet(key, value);
        }
    }

    public void DeleteKey(string key)
    {
        _ = db.KeyDelete(key);
    }
}

