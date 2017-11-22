using System;
using EntityFX.Gdcame.Infrastructure.Common;

#if NETSTANDARD2_0
        using Microsoft.Extensions.Caching.Memory;
#endif

#if NET461
using System.Runtime.Caching;
#endif

namespace EntityFX.Gdcame.Infrastructure
{
    public class Cache : ICache
    {

#if NETSTANDARD2_0
        private IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
#endif
#if NET461
        private readonly ObjectCache _cache = new MemoryCache("Cache");
#endif


        public Cache()
        {
        }

        public void Set(string key, object value, DateTimeOffset period)
        {
#if NETSTANDARD2_0
            this._cache.Set(key, value, period);
#endif
#if NET461
            this._cache.Set(key
                , value
                , new CacheItemPolicy { AbsoluteExpiration = period});
#endif
        }

        public object Get(string key)
        {
#if NETSTANDARD2_0
            return this._cache.Get(key);
#endif
#if NET461
            return this._cache.Get(key);
#endif
        }

        public bool Contains(string key)
        {
#if NET461
            return this._cache.Contains(key);
#endif
#if NETSTANDARD2_0
            object output;
            return this._cache.TryGetValue(key, out output);
#endif
        }

        public void Remove(string key)
        {
#if NET461
             this._cache.Remove(key);
#endif
#if NETSTANDARD2_0
            this._cache.Remove(key);
#endif
        }
    }
}