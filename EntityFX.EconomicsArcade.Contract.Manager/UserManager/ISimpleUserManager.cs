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
        void Create(string login);
    }
}