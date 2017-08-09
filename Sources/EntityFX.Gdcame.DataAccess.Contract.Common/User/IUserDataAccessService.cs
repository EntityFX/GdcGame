namespace EntityFX.Gdcame.DataAccess.Contract.Common.User
{
    using System.ServiceModel;

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
        User[] FindWithUsersIds(string[] userslds);

        [OperationContract]
        User[] FindAll();

        User[] FindChunked(int offset, int count);

        [OperationContract]
        int Count();
    }
}