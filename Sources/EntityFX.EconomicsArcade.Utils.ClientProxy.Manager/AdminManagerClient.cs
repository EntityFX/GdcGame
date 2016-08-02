using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class AdminManagerClient<TInfrastructureProxy> : IAdminManager
                                        where TInfrastructureProxy : InfrastructureProxy<IAdminManager>, new()
    {
        private readonly Guid _sesionGuid;
        private readonly Uri _endpointAddress;


        private readonly Action<Guid> operationContext;
        private ILogger _logger;
        private IOperationContextHelper _operationContextHelper;


        public AdminManagerClient(IOperationContextHelper operationContextHelper, string endpointAddress, Guid sessionGuid)
        {
            _sesionGuid = sessionGuid;
            _endpointAddress = new Uri(endpointAddress);
            _operationContextHelper = operationContextHelper;
            operationContext = _ => _operationContextHelper.Instance.SessionId = _;
        }

        public UserSessionsInfo[] GetActiveSessions()
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                var res = channel.GetActiveSessions();
                proxy.CloseChannel();
                return res;
            }
        }

        public void CloseSessionByGuid(Guid guid)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.CloseSessionByGuid(guid);
                proxy.CloseChannel();
            }
        }

        public void CloseAllUserSessions(string username)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.CloseAllUserSessions(username);
                proxy.CloseChannel();
            }
        }

        public void CloseAllSessions()
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.CloseAllSessions();
                proxy.CloseChannel();
            }
        }

        public void CloseAllSessionsExcludeThis(Guid guid)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.CloseAllSessionsExcludeThis(guid);
                proxy.CloseChannel();
            }
        }

        public void WipeUser(string username)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.WipeUser(username);
                proxy.CloseChannel();
            }
        }

        public void ReloadGame(string username)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.ReloadGame(username);
                proxy.CloseChannel();
            }
        }
    }
}
