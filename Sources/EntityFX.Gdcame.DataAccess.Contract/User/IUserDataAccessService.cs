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
        void Delete(string userId);

        [OperationContract]
        User FindById(string userId);

        [OperationContract]
        User FindByName(string name);

        [OperationContract]
        User[] FindByFilter(string searchString);

        [OperationContract]
        User[] FindAll();
    }
}