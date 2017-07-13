using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EntityFX.Gdcame.Utils.MainServer
{
    public class GameDataCachingInterceptionBehavior : IInterceptionBehavior, IDisposable
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
            var result = getNext()(input, getNext);
            AddToCache(objectName, result.ReturnValue);
            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }

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
                _memoryCache.Add(new CacheItem(key, toBeAddedToCache),
                    new CacheItemPolicy {AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1))});
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _memoryCache.Dispose();
                }
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GameDataCachingInterceptionBehavior() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}