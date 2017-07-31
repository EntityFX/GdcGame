namespace EntityFX.Gdcame.Utils.Common
{
    using System;

    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;

    public interface ISessionManagerClientFactory
    {
        ISessionManager BuildSessionManagerClient(Guid sessionGuid);
    }
}