using System;
using System.Configuration;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.UserRating;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;

using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;
using System.Data.Entity;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Mappers;
using EntityFX.EconomicsArcade.DataAccess.Service;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.FundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserRating;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.Counetrs;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserFundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserGameCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserRating;
using EntityFX.EconomicsArcade.DataAccess.Service.Mappers;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {

        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            var childBootstrappers = new IContainerBootstrapper[]
            {
                new EconomicsArcade.DataAccess.Repository.ContainerBootstrapper(),
                new EconomicsArcade.DataAccess.Service.ContainerBootstrapper()
            };

            childBootstrappers.ForEach(_ => _.Configure(container));
            container.AddNewExtension<Interception>();

            container.RegisterType<ILogger>(new InjectionFactory(
               _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
            {
                container.Configure<Interception>()
                .AddPolicy("logging")
                .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.EconomicsArcade.*", true));
            }

            if (!Environment.UserInteractive)
            {
                container.RegisterType<IServiceInfoHelper, ServiceInfoHelperLogger>();
            }
            else
            {
                container.RegisterType<IServiceInfoHelper, ServiceInfoHelperConsole>();
            }

            return container;
        }
    }
}
