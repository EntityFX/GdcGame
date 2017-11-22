using System;


namespace EntityFX.Gdcame.Infrastructure.Common
{

    public interface IResolver
    {
        T Resolve<T>(string name = null);

        T Resolve<T>(string name = null, params Tuple<string, object>[] constructorParameters);

        object Resolve(Type type, string name = null);

        object Resolve(Type type, string name = null, params object[] constructorParameters);
    }

    public interface IIocContainer : IResolver, IDisposable
    {
        void RegisterType<T>(ContainerScope scope = ContainerScope.Instance, string name = null);

        void RegisterType<T>(Func<IResolver,T> factory, ContainerScope scope = ContainerScope.Instance, string name = null);

        void RegisterType<T, U>(ContainerScope scope = ContainerScope.Instance, string name = null) where U : T;

        void RegisterType<T, U>(Func<IResolver, U> factory, ContainerScope scope = ContainerScope.Instance, string name = null) where U : T;

        void RegisterType(Type source, Type destination);
    }

    public interface IIocContainerDisposable<out TContainer>
    {
        TContainer ContainerBuilder { get; }
    }

    public interface IIocContainer<out TContainerBuilder, out TContainer> : IIocContainer, IIocContainerDisposable<TContainerBuilder>
    {
        TContainer Configure();
    }
}