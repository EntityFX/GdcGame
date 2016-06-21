using System;
using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
{
    [ServiceContract]
    public interface ISessionManager
    {
        [OperationContract]
        Guid AddSession(string login);
        [OperationContract]
        Session GetSession();
    }
}
