using System;
using System.Configuration;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.Utils.ClientProxy.Manager;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFx.EconomicsArcade.Test.Shared
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        private readonly bool _isCollapsed;

        public ContainerBootstrapper(bool isCollapsed)
        {
            _isCollapsed = isCollapsed;
        }

        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            container.RegisterType<IOperationContextHelper, WcfOperationContextHelper>();

            if (!_isCollapsed)
            {
                container.RegisterType<IGameManager, GameManagerClient<NetTcpProxy<IGameManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<ILogger>(), new ResolvedParameter<IOperationContextHelper>(),
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], typeof(Guid))
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISimpleUserManager, SimpleUserManagerClient<NetTcpProxy<ISimpleUserManager>>>(
                    new InjectionConstructor(
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"])
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );

                container.RegisterType<IAdminManager, AdminManagerClient<NetTcpProxy<IAdminManager>>>(
                    new InjectionConstructor(
                         new ResolvedParameter<IOperationContextHelper>(),
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_AdminManager"],
                        new ResolvedParameter<Guid>()),
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISessionManager, SessionManagerClient<NetTcpProxy<ISessionManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<IOperationContextHelper>(),
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"],
                        new ResolvedParameter<Guid>())
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
            }
            else
            {

                container.RegisterType<IGameManager, GameManagerClient<NetNamedPipeProxy<IGameManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<ILogger>(),
                        new ResolvedParameter<IOperationContextHelper>(),
                        "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.GameManager.IGameManager",
                        typeof(Guid))
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );

                container
                    .RegisterType<ISimpleUserManager, SimpleUserManagerClient<NetNamedPipeProxy<ISimpleUserManager>>>(
                        new InjectionConstructor(
                            "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.UserManager.ISimpleUserManager")
                        , new Interceptor<InterfaceInterceptor>()
                        , new InterceptionBehavior<LoggerInterceptor>()
                    );

                container.RegisterType<IAdminManager, AdminManagerClient<NetNamedPipeProxy<IAdminManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<IOperationContextHelper>(),
                        "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.AdminManager.IAdminManager",
                        new ResolvedParameter<Guid>()),
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISessionManager, SessionManagerClient<NetNamedPipeProxy<ISessionManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<IOperationContextHelper>(),
                        "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.SessionManager.ISessionManager",
                        new ResolvedParameter<Guid>())
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
            }

            return container;
        }
    }
}