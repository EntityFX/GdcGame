﻿using System;
using System.Net;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;

namespace EntityFX.Gdcame.Utils.ServiceStarter.WcfCollapsed
{
    public class CollapsedServiceStarter : ServicesStarterBase<CollapsedContainerBootstrapper>, IServicesStarter,
        IDisposable
    {
        private readonly Uri _baseMsmqUrl = new Uri("net.msmq://localhost/private/");
        private readonly string _baseUrl = "net.tcp://localhost";
        private readonly string _signalRHost = "http://+:8088/";
        private IDisposable _webApp;

        public CollapsedServiceStarter(CollapsedContainerBootstrapper bootstrapper)
            : base(bootstrapper)
        {
        }

        public void Dispose()
        {
            _webApp.Dispose();
        }

        public override void StartServices()
        {
            AddNetTcpService<IUserDataAccessService>(new Uri(_baseUrl + ":11000"));
            AddNetTcpService<IGameDataRetrieveDataAccessService>(new Uri(_baseUrl + ":11001"));
            AddNetTcpService<IGameDataStoreDataAccessService>(new Uri(_baseUrl + ":11002"));

            AddNetTcpService<ISessionManager>(new Uri(_baseUrl + ":10000"));
            AddNetTcpService<ISimpleUserManager>(new Uri(_baseUrl + ":10001"));
            AddNetTcpService<IRatingManager>(new Uri(_baseUrl + ":10002"));
            AddCustomService<GameManagerTcpServiceHost>(new Uri(_baseUrl + ":10003"));
            AddCustomService<AdminManagerTcpServiceHost>(new Uri(_baseUrl + ":10004"));

            AddCustomService<NotifyConsumerServiceHost>(new Uri(_baseUrl + ":10005"));

            _webApp = WebApp.Start(_signalRHost, builder =>
            {
                var listener = (HttpListener) builder.Properties[typeof (HttpListener).FullName];
                listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
                builder.UseCors(CorsOptions.AllowAll);
                builder.MapSignalR();
                builder.RunSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJSONP = true
                });
            });
            Console.WriteLine("Server running on {0}", _signalRHost);
            OpenServices();
        }

        public override void StopServices()
        {
            CloseServices();
        }

        protected override void OnServiceOpened(IServiceHost service)
        {
            var serviceInfoHelper = _container.Resolve<IServiceInfoHelper>();
            serviceInfoHelper.PrintServiceHostInfo(service.ServiceHost);
            //ServiceInfoHelperConsole.PrintServiceHostInfo(service.ServiceHost);
        }
    }
}