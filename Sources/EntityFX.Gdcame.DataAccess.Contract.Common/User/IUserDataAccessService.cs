namespace EntityFX.Gdcame.DataAccess.Contract.Common.User
{
    //

    //
    public interface IUserDataAccessService
    {
        //
        int Create(User user);

        //
        void Update(User user);

        //
        void Delete(string userId);

        //
        User FindById(string userId);

        //
        User FindByName(string name);

        //
        User[] FindByFilter(string searchString);

        //
        User[] FindWithUsersIds(string[] userslds);

        //
        User[] FindAll();

        User[] FindChunked(int offset, int count);

        //
        int Count();
    }
}