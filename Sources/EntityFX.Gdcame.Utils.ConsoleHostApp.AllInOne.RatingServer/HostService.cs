using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;

using Topshelf;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.RatingServer
{
    using EntityFX.Gdcame.Engine.Worker.RatingServer;

    internal class HostService : HostServiceBase<CoreStartup>
    {
        protected override IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration)
        {
            return new ContainerBootstrapper(appConfiguration);
        }

        protected override void ConfigureWorkers(IWorkerManager workerManager, IUnityContainer container)
        {
            workerManager.Add(container.Resolve<RatingSyncWorker>());
        }
    }
}
