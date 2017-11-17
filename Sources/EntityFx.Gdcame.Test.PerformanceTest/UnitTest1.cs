using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using EntityFX.Gdcame.DataAccess.Service;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;
using EntityFX.Gdcame.Manager.MainServer;
using EntityFX.Gdcame.Manager.MainServer.Workers;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Utils.Hashing;
using EntityFX.Gdcame.Utils.MainServer;
using Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFX.Gdcame.Infrastructure;

namespace EntityFx.Gdcame.Test.Unit
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.DataAccess.Service.Common;
    using EntityFX.Gdcame.DataAccess.Service.MainServer;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Engine.GameEngine;
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;
    using EntityFX.Gdcame.Engine.Worker.MainServer;
    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;

    using ContainerBootstrapper = EntityFX.Gdcame.Manager.MainServer.ContainerBootstrapper;

    [TestClass]
    public class UnitTest1 : IDisposable
    {
        private readonly IIocContainer container = new UnityIocContainer(new UnityContainer());

        [TestInitialize]
        public void TestInitialize()
        {
            this.container.RegisterType<ILogger>(() => new Logger(new NLoggerAdapter(NLog.LogManager.GetLogger("logger"))));

            this.container.RegisterType<IOperationContextHelper>(() => new FakeOperationContextHelper(), ContainerScope.Singleton);
            var managerBootstrapper = new ContainerBootstrapper();
            managerBootstrapper.Configure(this.container);

            var childBootstrappers = new IContainerBootstrapper[]
            {
                new EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.ContainerBootstrapper(),
                new EntityFX.Gdcame.DataAccess.Service.MainServer.ContainerBootstrapper(),
                new EntityFX.Gdcame.NotifyConsumer.ContainerBootstrapper()
            };
            Array.ForEach(childBootstrappers, _ => _.Configure(this.container));

            this.container.RegisterType<IGameFactory, GameFactory>();

            this.container.RegisterType<IGameDataPersister, GameDataPersister>();

            this.container.RegisterType<IGameDataPersisterFactory, GameDataPersisterFactory>();

            this.container.RegisterType<IHashHelper, HashHelper>();

            this.container.RegisterType(() => new GamePerformanceInfo(), ContainerScope.Singleton);

            this.container.RegisterType( () => 
                new GameSessions(this.container.Resolve<ILogger>(), this.container.Resolve<IGameFactory>(), this.container.Resolve<GamePerformanceInfo>()), ContainerScope.Singleton);

            var workers = new List<IWorker>();
            workers.Add(this.container.Resolve<CalculationWorker>());
            workers.Add(this.container.Resolve<PersistenceWorker>());
            workers.Add(this.container.Resolve<SessionValidationWorker>());
            workers.ForEach(_ => _.Run<object>());

            this.container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessDocumentService>(
                );
            this.container.RegisterType<IUserDataAccessService, UserDataAccessService>(
                );
            this.container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessDocumentService>(
                );

            this.container.RegisterType<INotifyConsumerService, FakeNotifyConsumerService>();

            this.container.RegisterType<IGameDataChangesNotifier, GameDataChangesNotifier>();

            this.container.RegisterType<INotifyConsumerClientFactory, NotifyConsumerClientFactory>(() => new NotifyConsumerClientFactory(container));
        }

        [TestMethod]
        public void TestIntegrity()
        {
            var userName = "test-user-1";
            var su = this.container.Resolve<ISimpleUserManager>();
            if (!su.Exists(userName))
            {
                su.Create(new UserData {Login = userName});
            }

            var sm = this.container.Resolve<ISessionManager>();
            var sessionGuid = sm.OpenSession(userName);
            var och = this.container.Resolve<IOperationContextHelper>();
            och.Instance.SessionId = sessionGuid;
            var gm = this.container.Resolve<IGameManager>();
            gm.Ping();
        }


        [TestMethod]
        public void TestEngine()
        {
            var gf = this.container.Resolve<IGameFactory>();
            var game = gf.BuildGame("1", "test-user-1");
            game.Initialize();
            game.PerformManualStep(null);
            var csw = new Stopwatch();
            foreach (var iterNumber in new[] {1, 10, 50, 100, 500, 1000, 5000, 10000, 50000})
            {
                var swList = Enumerable.Repeat(new Stopwatch(), iterNumber).ToArray();
                csw.Restart();
                for (var iteration = 0; iteration < iterNumber; iteration++)
                {
                    swList[iteration].Start();
                    game.PerformAutoStep();
                    swList[iteration].Stop();
                }

                Debug.Print("Ellapsed for {0} iterrations: {1}", iterNumber, csw.Elapsed);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.container.Dispose();
                }

                this.disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitTest1() {
        // // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        // Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            this.Dispose(true);

            // GC.SuppressFinalize(this);
        }

        #endregion
    }

    internal class FakeOperationContextHelper : IOperationContext, IOperationContextHelper
    {
        private static readonly Lazy<IOperationContext> ObjInstance =
            new Lazy<IOperationContext>(() => new FakeOperationContextHelper());

        public Guid? SessionId { get; set; }

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