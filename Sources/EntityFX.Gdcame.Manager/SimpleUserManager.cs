using System;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.UserManager;

namespace EntityFX.Gdcame.Manager
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