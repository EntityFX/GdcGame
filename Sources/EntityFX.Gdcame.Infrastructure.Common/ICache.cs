namespace EntityFX.Gdcame.Infrastructure.Common
{
    using System;

    public interface ICache
    {
        void Set(string key, object value, DateTimeOffset period);

        object Get(string key);

        void Remove(string key);

        bool Contains(string key);
    }
}