using System;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class SessionManagerClient
    {

        private const string EndpointAddress = "net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.SessionManager.ISessionManager";

        public Guid AddSession(string login)
        {
            using (var proxy = new SessionManagerProxy(Guid.Empty))
            {
                var channel = proxy.CreateChannel(new Uri(EndpointAddress));
                proxy.ApplyContextScope();
                var res = channel.AddSession(login);
                proxy.CloseChannel();
                return res;
            }
        }

        public Session GetSession(Guid sessionGuid)
        {
            using (var proxy = new SessionManagerProxy(sessionGuid))
            {
                var channel = proxy.CreateChannel(new Uri(EndpointAddress));
                proxy.ApplyContextScope();
                var res = channel.GetSession();
                proxy.CloseChannel();
                return res;
            }
        }
    }
}