﻿using System;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Manager.Contract.AdminManager;

namespace EntityFX.Gdcame.Utils.ClientProxy.WcfManager
{
    public class AdminManagerClient<TInfrastructureProxy> : IAdminManager
        where TInfrastructureProxy : IInfrastructureProxy<IAdminManager, Binding>, new()
    {
        private readonly Uri _endpointAddress;
        private readonly Guid _sesionGuid;


        private readonly Action<Guid> operationContext;
        private readonly IOperationContextHelper _operationContextHelper;


        public AdminManagerClient(IOperationContextHelper operationContextHelper, string endpointAddress,
            Guid sessionGuid)
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