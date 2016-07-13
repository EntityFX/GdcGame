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
        private readonly Uri _endpointAddress;

        public AdminManagerClient(string endpointAddress)
        {
            _endpointAddress = new Uri(endpointAddress);
        }

        public UserSessionsInfo[] GetActiveSessions()
        {
            using (var proxy = new AdminManagerProxy())
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
            using (var proxy = new AdminManagerProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.CloseSessionByGuid(guid);
                proxy.CloseChannel();
            }
        }

        public void CloseAllUserSessions(string username)
        {
            using (var proxy = new AdminManagerProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.CloseAllUserSessions(username);
                proxy.CloseChannel();
            }
        }

        public void WipeUser(string username)
        {
            using (var proxy = new AdminManagerProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.WipeUser(username);
                proxy.CloseChannel();
            }
        }
    }
}
