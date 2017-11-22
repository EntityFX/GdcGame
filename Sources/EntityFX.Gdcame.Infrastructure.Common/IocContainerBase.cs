using System;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public abstract class IocContainerBase<TContainer> : IIocContainerDisposable<TContainer>
    {
        private bool _disposed = false;

        /*public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Container.Dispose();
            }
            _disposed = true;
        }*/

        public abstract TContainer ContainerBuilder { get; }
    }
}