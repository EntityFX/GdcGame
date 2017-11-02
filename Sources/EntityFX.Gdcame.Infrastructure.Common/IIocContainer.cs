using System;


namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IIocContainer : IDisposable
    {
        T Resolve<T>(string name = null);

        T Resolve<T>(string name = null, params Tuple<string, object>[] constructorParameters);

        object Resolve(Type type, string name = null);

        object Resolve(Type type, string name = null, params object[] constructorParameters);

        void RegisterType<T>(ContainerScope scope = ContainerScope.Instance, string name = null);

        void RegisterType<T>(Func<T> factory, ContainerScope scope = ContainerScope.Instance, string name = null);

        void RegisterType<T, U>(ContainerScope scope = ContainerScope.Instance, string name = null) where U : T;

        void RegisterType<T, U>(Func<U> factory, ContainerScope scope = ContainerScope.Instance, string name = null) where U : T;
    }

    public interface IIocContainerDisposable<out TContainer>
        where TContainer : IDisposable
    {
        TContainer Container { get; }
    }

    public interface IIocContainer<out TContainer> : IIocContainer, IIocContainerDisposable<TContainer>
    where TContainer : IDisposable
    {
    }
}