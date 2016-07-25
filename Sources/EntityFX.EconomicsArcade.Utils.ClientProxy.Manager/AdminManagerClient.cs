using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class AdminManagerClient : IAdminManager
    {
        private readonly Guid _sessionGuid;
        private readonly Uri _endpointAddress;

        public AdminManagerClient(string endpointAddress, Guid sessionGuid)
        {
            _sessionGuid = sessionGuid;
            _endpointAddress = new Uri(endpointAddress);
        }

        public UserSessionsInfo[] GetActiveSessions()
        {
            using (var proxy = new AdminManagerProxy(_sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.GetActiveSessions();
                proxy.CloseChannel();
                return res;
            }
        }

        public void CloseSessionByGuid(Guid guid)
        {
            using (var proxy = new AdminManagerProxy(_sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.CloseSessionByGuid(guid);
                proxy.CloseChannel();
            }
        }

        public void CloseAllUserSessions(string username)
        {
            using (var proxy = new AdminManagerProxy(_sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.CloseAllUserSessions(username);
                proxy.CloseChannel();
            }
        }

        public void CloseAllSessions()
        {
            using (var proxy = new AdminManagerProxy(_sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.CloseAllSessions();
                proxy.CloseChannel();
            }
        }

        public void CloseAllSessionsExcludeThis(Guid guid)
        {
            using (var proxy = new AdminManagerProxy(_sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.CloseAllSessionsExcludeThis(guid);
                proxy.CloseChannel();
            }
        }

        public void WipeUser(string username)
        {
            using (var proxy = new AdminManagerProxy(_sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.WipeUser(username);
                proxy.CloseChannel();
            }
        }

        public void ReloadGame(string username)
        {
            using (var proxy = new AdminManagerProxy(_sessionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.ReloadGame(username);
                proxy.CloseChannel();
            }
        }
    }
}
