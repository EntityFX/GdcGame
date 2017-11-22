using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using RestSharp.Authenticators;
using Topshelf;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.RatingServer
{
    using EntityFX.Gdcame.Engine.Worker.RatingServer;

    internal class HostService : HostServiceBase<CoreStartup>
    {
        public HostService(IRuntimeHelper runtimeHelper, AppConfiguration appConfiguration, IIocContainer container) : base(runtimeHelper, appConfiguration, container)
        {
        }

        protected override IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration, IIocContainer iocContainer)
        {
            return new ContainerBootstrapper<IAuthenticator>(appConfiguration, iocContainer.Resolve<IRuntimeHelper>());
        }

        protected override void ConfigureWorkers(IWorkerManager workerManager, IIocContainer container)
        {
            workerManager.Add(container.Resolve<RatingSyncWorker>());
        }
    }
}
