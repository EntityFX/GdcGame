using System;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager
{
    public class SimpleUserManager : ISimpleUserManager
    {
        private readonly IUserDataAccessService _userDataAccess;
        private readonly ILogger _logger;

        public SimpleUserManager(ILogger logger, IUserDataAccessService userDataAccess)
        {
            _logger = logger;
            _userDataAccess = userDataAccess;
        }

        public bool Exists(string login)
        {
            var user = _userDataAccess.FindByName(login);
            return user != null;
        }

        public UserData FindById(int id)
        {
            var user = _userDataAccess.FindById(id);
            return user != null ? new UserData() { Id = user.Id, Login = user.Email, PasswordHash = user.PasswordHash } : null;
        }

        public UserData Find(string login)
        {
            var user = _userDataAccess.FindByName(login);
            return user != null ? new UserData() { Id = user.Id, Login = user.Email, PasswordHash = user.PasswordHash } : null;
        }

        public void Create(UserData login)
        {
            try
            {
                _userDataAccess.Create(new User() { Email = login.Login, IsAdmin = false, PasswordHash = login.PasswordHash});
            }
            catch (Exception exp)
            {
                
            }
        }

        public void Delete(int id)
        {
            _userDataAccess.Delete(id);
        }
    }
}