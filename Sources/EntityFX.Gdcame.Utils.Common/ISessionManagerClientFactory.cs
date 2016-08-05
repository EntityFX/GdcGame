using System;
using EntityFX.Gdcame.Manager.Contract.SessionManager;

namespace EntityFX.Gdcame.Utils.Common
{
    public interface ISessionManagerClientFactory
    {
        ISessionManager BuildSessionManagerClient(Guid sessionGuid);   
    }
}