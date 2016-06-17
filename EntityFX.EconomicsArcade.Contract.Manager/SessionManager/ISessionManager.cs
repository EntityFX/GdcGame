using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
{
    [ServiceContract]
    public interface ISessionManager
    {
        [OperationContract]
        Guid AddSession(string login);

        Session GetSession();
    }
}
