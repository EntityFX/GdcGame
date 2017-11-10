namespace EntityFX.Gdcame.Manager.Contract.Common.UserManager
{
    //

    using EntityFX.Gdcame.Contract.Common;

    /// <summary>
    /// </summary>
    //
    public interface ISimpleUserManager
    {
        //
        bool Exists(string login);

        //
        UserData FindById(string id);

        //
        UserData Find(string login);

        //
        UserData[] FindByFilter(string searchString);

        //
        void Create(UserData login);

        //
        void Delete(string id);
    }
}