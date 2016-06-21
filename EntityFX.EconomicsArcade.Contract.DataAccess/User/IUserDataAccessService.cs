using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.DataAccess.User
{
    [ServiceContract]
    public interface IUserDataAccessService
    {
        [OperationContract]
        int Create(User user);
        [OperationContract]
        void Update(User user);
        [OperationContract]
        void Delete(int userId);
        [OperationContract]
        User FindById(int userId);
        [OperationContract]
        User[] FindAll();
    }
}
