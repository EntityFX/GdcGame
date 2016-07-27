﻿using System;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class SessionManagerClient<TInfrastructureProxy>     : ISessionManager
                        where TInfrastructureProxy : InfrastructureProxy<ISessionManager>, new()
    {
        private readonly Guid _sessionGuid;
        private readonly Uri _endpointAddress;// = "net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.SessionManager.ISessionManager";

        private static readonly Action<Guid> operationContext = (_) =>
        {
            OperationContextHelper.Instance.SessionId = _;
        };

        public SessionManagerClient(string endpointAddress, Guid sessionGuid)
        {
            _sessionGuid = sessionGuid;
            _endpointAddress = new Uri(endpointAddress);
        }

        public Guid OpenSession(string login)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, Guid.Empty);
                var res = channel.OpenSession(login);
                proxy.CloseChannel();
                return res;
            }
        }

        public Session GetSession()
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sessionGuid);
                var res = channel.GetSession();
                proxy.CloseChannel();
                return res;
            }
        }

        public bool CloseSession()
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sessionGuid);
                var res = channel.CloseSession();
                proxy.CloseChannel();
                return res;
            }
        }
    }
}