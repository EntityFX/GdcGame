using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.NetNamedPipe;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
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

            if (!_isCollapsed)
            {
                container.RegisterType<IGameManager, GameManagerClient<NetTcpProxy<IGameManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<ILogger>(),
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], typeof (Guid))
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
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_AdminManager"],
                        new ResolvedParameter<Guid>()),
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISessionManager, SessionManagerClient<NetTcpProxy<ISessionManager>>>(
                    new InjectionConstructor(
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
                        "net.pipe://localhost/EntityFX.EconomicsArcade.Contract.Manager.GameManager.IGameManager",
                        typeof (Guid))
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );

                container
                    .RegisterType<ISimpleUserManager, SimpleUserManagerClient<NetNamedPipeProxy<ISimpleUserManager>>>(
                        new InjectionConstructor(
                            "net.pipe://localhost/EntityFX.EconomicsArcade.Contract.Manager.UserManager.ISimpleUserManager")
                        , new Interceptor<InterfaceInterceptor>()
                        , new InterceptionBehavior<LoggerInterceptor>()
                    );

                container.RegisterType<IAdminManager, AdminManagerClient<NetNamedPipeProxy<IAdminManager>>>(
                    new InjectionConstructor(
                        "net.pipe://localhost/EntityFX.EconomicsArcade.Contract.Manager.AdminManager.IAdminManager",
                        new ResolvedParameter<Guid>()),
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISessionManager, SessionManagerClient<NetNamedPipeProxy<ISessionManager>>>(
                    new InjectionConstructor(
                        "net.pipe://localhost/EntityFX.EconomicsArcade.Contract.Manager.SessionManager.ISessionManager",
                        new ResolvedParameter<Guid>())
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
            }

            return container;
        }
    }
}