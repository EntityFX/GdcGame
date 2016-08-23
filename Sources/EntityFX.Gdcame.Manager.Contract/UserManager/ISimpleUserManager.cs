using System.ServiceModel;

namespace EntityFX.Gdcame.Manager.Contract.UserManager
{
    /// <summary>
    /// </summary>
    [ServiceContract]
    public interface ISimpleUserManager
    {
        [OperationContract]
        bool Exists(string login);

        [OperationContract]
        UserData FindById(string id);

        [OperationContract]
        UserData Find(string login);

        [OperationContract]
        UserData[] FindByFilter(string searchString);

        [OperationContract]
        void Create(UserData login);

        [OperationContract]
        void Delete(string id);
    }
}