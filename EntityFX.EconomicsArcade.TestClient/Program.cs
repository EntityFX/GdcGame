using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;

namespace EntityFX.EconomicsArcade.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid ses;
            using (var proxyFactory = new SessionManagerProxyFactory(Guid.Empty))
            {
                var proxy = proxyFactory.Create(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.Manager:8555/EntityFX.EconomicsArcade.Contract.Manager.SessionManager.ISessionManager"));
                ses = proxy.AddSession("entityfx");
            }

            using (var proxyFactory = new SessionManagerProxyFactory(ses))
            {
                var proxy = proxyFactory.Create(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.Manager:8555/EntityFX.EconomicsArcade.Contract.Manager.SessionManager.ISessionManager"));
                var ss = proxy.GetSession();
            }

            using (var proxyFactory = new GameManagerProxyFactoy(ses))
            {
                var proxy = proxyFactory.Create(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.Manager:8555/EntityFX.EconomicsArcade.Contract.Manager.GameManager.IGameManager"));
                var gameData = proxy.GetGameData();
                proxy.PerformManualStep();
                var counters = proxy.GetCounters();
            }
        }
    }
}
