using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EntityFX.Gdcame.Utils.Common
{
    public class GameDataCachingInterceptionBehavior : IInterceptionBehavior
    {
        private readonly MemoryCache _memoryCache = new MemoryCache("GameDataCache");

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var objectName = getNext.Target.ToString();
            if (input.MethodBase.Name == "GetGameData" && getNext.Method.Name == "FindAll")
            {
                if (IsInCache(objectName))
                {
                    return input.CreateMethodReturn(
                        FetchFromCache(objectName));
                }
            }
            IMethodReturn result = getNext()(input, getNext);
            AddToCache(objectName, result.ReturnValue);
            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute { get { return true; } }

        private bool IsInCache(string key)
        {
            return _memoryCache.Contains(key);
        }

        private object FetchFromCache(string key)
        {
            return _memoryCache.Contains(key) ? _memoryCache.Get(key) : null;
        }

        private void AddToCache(string key, object toBeAddedToCache)
        {
            if (!_memoryCache.Contains(key))
                _memoryCache.Add(new CacheItem(key, toBeAddedToCache), new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) });
        }
    }
}
