using System;
using System.Configuration;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Common.Presentation.Model.Mappers;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Presentation.Web.Api.Models;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Presentation.Web.Controller;
using EntityFX.Gdcame.Presentation.Web.Model;
using EntityFX.Gdcame.Presentation.Web.Model.Mappers;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.ServiceStarter.Collapsed;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            //container.AddNewExtension<Interception>();

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            new FullCollapsedContainerBootstrapper().Configure(container);


            container.RegisterType<IMapper<Cash, CashModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<Item, ItemModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();
            container.RegisterType<IMapper<BuyFundDriverResult, BuyDriverModel>, FundsDriverBuyInfoMapper>();
            container.RegisterType<IGameClientFactory, NoWcfGameManagerFactory>();
            container.RegisterType<ISessionManagerClientFactory, SessionManagerClientFactory>();

            container.RegisterType<ApplicationUserManagerFacotory>();
            container.RegisterType<UserManager<GameUser>, ApplicationUserManager>();
            container.RegisterType<IUserStore<GameUser>, GameUserStore>();
            //container.RegisterType<IAccountController,AuthController>();
            container.RegisterType<IGameDataProvider, GameDataProvider>();

            container.RegisterType<IGameApiController, GameApiController>();
            container.RegisterType<IRatingApiController, RatingApiController>();
            container.RegisterType<IAccountController, AccountController>();
            container.RegisterType<IOperationContextHelper, NoWcfOperationContextHelper>();

            return container;
        }
    }
}