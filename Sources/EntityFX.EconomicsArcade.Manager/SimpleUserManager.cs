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

        public void Create(string login)
        {
            try
            {
                _userDataAccess.Create(new User() { Email = login });
            }
            catch (Exception exp)
            {
                
            }
        }
    }
}