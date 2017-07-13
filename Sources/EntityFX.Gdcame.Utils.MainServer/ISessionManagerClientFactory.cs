using System;
using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;

namespace EntityFX.Gdcame.Utils.MainServer
{
    public interface ISessionManagerClientFactory
    {
        ISessionManager BuildSessionManagerClient(Guid sessionGuid);
    }
}