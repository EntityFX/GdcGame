using System;
using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Contract.Manager.UserManager
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface ISimpleUserManager
    {
        [OperationContract]
        bool Exists(string login);
        [OperationContract]
        UserData FindById(int id);
        [OperationContract]
        UserData Find(string login);
        [OperationContract]
        void Create(UserData login);
        [OperationContract]
        void Delete(int id);
    }
}