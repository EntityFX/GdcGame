using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Manager.AdminManager
{
    [ServiceContract]
    public interface IAdminManager
    {
        [OperationContract]
        UserSessionsInfo[] GetActiveSessions();
        [OperationContract]
        void CloseSessionByGuid(Guid guid);
        [OperationContract]
        void CloseAllUserSessions(string username);
        [OperationContract]
        void WipeUser(string username);
    }
}
