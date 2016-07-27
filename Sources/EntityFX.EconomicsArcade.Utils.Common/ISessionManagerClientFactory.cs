using System;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Utils.Common
{
    public interface ISessionManagerClientFactory
    {
        ISessionManager BuildSessionManagerClient(Guid sessionGuid);   
    }
}