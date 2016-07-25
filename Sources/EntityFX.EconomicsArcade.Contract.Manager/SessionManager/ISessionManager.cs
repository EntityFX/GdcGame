using System;
using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
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
