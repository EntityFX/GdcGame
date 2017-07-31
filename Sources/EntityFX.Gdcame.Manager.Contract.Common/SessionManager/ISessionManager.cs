namespace EntityFX.Gdcame.Manager.Contract.Common.SessionManager
{
    using System;
    using System.ServiceModel;

    using EntityFX.Gdcame.Contract.Common;

    [ServiceContract]
    public interface ISessionManager
    {
        [OperationContract]
        Guid OpenSession(string login);

        [OperationContract]
        bool CloseSession();

        [OperationContract]
        Session GetSession();
    }
}