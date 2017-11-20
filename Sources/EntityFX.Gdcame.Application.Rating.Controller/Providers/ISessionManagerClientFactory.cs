using System;
using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    public interface ISessionManagerClientFactory
    {
        ISessionManager BuildSessionManagerClient(Guid sessionGuid);
    }
}