namespace EntityFX.Gdcame.Utils.Common
{
    using System;
    using System.Runtime.Caching;

    using EntityFX.Gdcame.Infrastructure.Common;
    public class Cache : ICache
    {
        private readonly ObjectCache _cache = MemoryCache.Default;

        public void Set(string key, object value, DateTimeOffset period)
        {
            this._cache.Set(key
                , value
                , new CacheItemPolicy { AbsoluteExpiration = period});
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public bool Contains(string key)
        {
            return this._cache.Contains(key);
        }

        public void Remove(string key)
        {
            this._cache.Remove(key);
        }
    }
}