using System.Diagnostics.CodeAnalysis;
using DBetter.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace DBetter.Infrastructure.Caching;

public class Cache(IMemoryCache cache) : ICache
{
    public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        return cache.TryGetValue(key, out value);
    }

    public void Set<T>(string key, T value)
    {
        Set(key, value, CachingOptions.Default);
    }

    public void Set<T>(string key, T value, CachingOptions options)
    {
        cache.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = options.Duration
        });
    }

    public void Remove(string key)
    {
        cache.Remove(key);
    }
}