using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Service;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.NotifyConsumer;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortableLog.NLog;
using ContainerBootstrapper = EntityFX.Gdcame.Manager.ContainerBootstrapper;

namespace EntityFx.Gdcame.Test.PerformanceTest
{
    [TestClass]
    public class UnitTest1
    {
        IUnityContainer container = new UnityContainer(); 
        
        [TestInitialize]
        public void TestInitialize()
        {
            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter(new NLogLogExFactory().GetLogger("logger")))));

            container.RegisterInstance<IOperationContextHelper>(new FakeOperationContextHelper());
                              ContainerBootstrapper managerBootstrapper = new ContainerBootstrapper();
            managerBootstrapper.Configure(container);

            var childBootstrappers = new IContainerBootstrapper[]
            {
                new EntityFX.Gdcame.DataAccess.Repository.ContainerBootstrapper(),
                new EntityFX.Gdcame.DataAccess.Service.ContainerBootstrapper(),
                new EntityFX.Gdcame.NotifyConsumer.ContainerBootstrapper(),
            };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));

            container.RegisterType<IGameFactory, GameFactory>();

            container.RegisterInstance<GameSessions>(new GameSessions(container.Resolve<ILogger>(), container.Resolve<IGameFactory>()));

            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessDocumentService>(
   );
            container.RegisterType<IUserDataAccessService, UserDataAccessService>(

                );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessDocumentService>(
                );

            container.RegisterType<INotifyConsumerService, FakeNotifyConsumerService>();

            container.RegisterType<INotifyGameDataChanged, NotifyGameDataChanged>(
                new InjectionConstructor(
                    new ResolvedParameter<int>(),
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapper<IGame, GameData>>("GameDataMapper"),
                    new ResolvedParameter<IMapper<FundsDriver, EntityFX.Gdcame.Common.Contract.Funds.FundsDriver>>(),
                    new ResolvedParameter<INotifyConsumerClientFactory>()
                    )
                );

            container.RegisterType<INotifyConsumerClientFactory, NotifyConsumerClientFactory>(new InjectionConstructor(
        new ResolvedParameter<IUnityContainer>(),
        string.Empty));
        }

        [TestMethod]
        public void TestIntegrity()
        {
            
            var userName = "test-user-1";
            ISimpleUserManager su = container.Resolve<ISimpleUserManager>();
            if (!su.Exists(userName))
            {
                su.Create(new UserData() { Login = userName});
            }
            ISessionManager sm = container.Resolve<ISessionManager>();
            var sessionGuid = sm.OpenSession(userName);
            var och = container.Resolve<IOperationContextHelper>();
            och.Instance.SessionId = sessionGuid; 
            IGameManager gm = container.Resolve<IGameManager>();
            gm.Ping();
        }


        [TestMethod]
        public void TestEngine()
        {
            var gf = container.Resolve<IGameFactory>();
            var game = gf.BuildGame(1, "test-user-1");
            game.Initialize();
            game.PerformManualStep(null);
            var csw = new Stopwatch();
            foreach (var iterNumber in new [] {1, 10, 50, 100, 500, 1000, 5000, 10000})
            {
                var swList = Enumerable.Repeat(new Stopwatch(), iterNumber).ToArray();
                csw.Restart();
                for (var iteration = 0; iteration < iterNumber; iteration++)
                {
                    swList[iteration].Start();
                    game.PerformAutoStep().Wait();
                    swList[iteration].Stop();
                }
                Debug.Print("Ellapsed for {0} iterrations: {1}", iterNumber, csw.Elapsed);
            }



        }
    }

    internal class FakeOperationContextHelper : IOperationContext, IOperationContextHelper
    {
        public Guid? SessionId { get; set; }
        private static readonly Lazy<IOperationContext> ObjInstance =
            new Lazy<IOperationContext>(() => new FakeOperationContextHelper());

        public IOperationContext Instance
        {
            get { return ObjInstance.Value; }
        }
    }

    internal class FakeNotifyConsumerService : INotifyConsumerService
    {
        public void PushGameData(UserContext userContext, GameData gameData)
        {

        }
    }
}
