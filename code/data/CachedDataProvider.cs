using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace data;

/// <summary>
/// Reading from disks is slow, keeping (some) data is memory is faster.
/// By using the MemoryCache we can significantly reduce api call times but using more memory as a tradeoff.
/// </summary>
public class CachedDataProvider : IDataProvider
{
    /// <summary>
    /// Configure how long we allow a cached item to be kept in memory when it is not accessed.
    /// </summary>
    private static readonly TimeSpan SlidingExpiry = TimeSpan.FromMinutes(10);
    /// <summary>
    /// Configure how long we allow data to be cached
    /// </summary>
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(60);
    
    private readonly IMemoryCache _memoryCache;
    public CachedDataProvider(IMemoryCache cache)
    {
        _memoryCache = cache;
    }

    public IEnumerable<CameraData> GetCameras()
    {
        return _memoryCache.GetOrCreate(
            "Cameras", 
            (entry) => {
                entry.SlidingExpiration = SlidingExpiry;
                entry.AbsoluteExpirationRelativeToNow = Expiry;
                return new CsvDataProvider().GetCameras();
            });
    }
}