using System;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class SessionManagerClient
    {
        private readonly Uri _endpointAddress;// = "net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.SessionManager.ISessionManager";

        public SessionManagerClient(string endpointAddress)
        {
            _endpointAddress = new Uri(endpointAddress);
        }

        public Guid AddSession(string login)
        {
            using (var proxy = new SessionManagerProxy(Guid.Empty))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.OpenSession(login);
                proxy.CloseChannel();
                return res;
            }
        }

        public Session GetSession(Guid sessionGuid)
        {
            using (var proxy = new SessionManagerProxy(sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.GetSession();
                proxy.CloseChannel();
                return res;
            }
        }

        public bool CloseSession(Guid sessionGuid)
        {
            using (var proxy = new SessionManagerProxy(sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.CloseSession();
                proxy.CloseChannel();
                return res;
            }
        }
    }
}