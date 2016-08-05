using System.ServiceModel;

namespace EntityFX.Gdcame.DataAccess.Contract.User
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
        User FindByName(string name);
        [OperationContract]
        User[] FindAll();
    }
}
