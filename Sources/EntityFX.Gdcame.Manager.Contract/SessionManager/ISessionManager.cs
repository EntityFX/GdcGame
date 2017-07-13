using System;
using System.ServiceModel;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager
{
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