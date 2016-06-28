using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;

namespace EntityFX.EconomicsArcade.Manager
{
    public class SimpleUserManager : ISimpleUserManager
    {
        private readonly IUserDataAccessService _userDataAccess;

        public SimpleUserManager(IUserDataAccessService userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        public bool Exists(string login)
        {
            var user = _userDataAccess.FindByName(login);
            return user != null;
        }

        public void Create(string login)
        {
            _userDataAccess.Create(new User() { Email = login});
        }
    }
}