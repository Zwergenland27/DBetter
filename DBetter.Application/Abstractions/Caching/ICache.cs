using System.Diagnostics.CodeAnalysis;

namespace DBetter.Application.Abstractions.Caching;

public interface ICache
{
    bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value);

    void Set<T>(string key, T value);
    void Set<T>(string key, T value, CachingOptions options);
    
    void Remove(string key);
}