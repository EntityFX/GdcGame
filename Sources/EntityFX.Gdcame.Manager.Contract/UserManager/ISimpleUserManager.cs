using System.ServiceModel;

namespace EntityFX.Gdcame.Manager.Contract.UserManager
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