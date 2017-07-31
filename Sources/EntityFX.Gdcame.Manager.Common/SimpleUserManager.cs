namespace EntityFX.Gdcame.Manager.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;

    using User = EntityFX.Gdcame.DataAccess.Contract.Common.User.User;

    public class SimpleUserManager : ISimpleUserManager
    {
        private readonly IHashHelper _hashHelper;
        private readonly ILogger _logger;
        private readonly IUserDataAccessService _userDataAccess;

        public SimpleUserManager(ILogger logger, IUserDataAccessService userDataAccess, IHashHelper hashHelper)
        {
            this._logger = logger;
            this._userDataAccess = userDataAccess;
            this._hashHelper = hashHelper;
        }

        public bool Exists(string login)
        {
            var user = this._userDataAccess.FindByName(login);
            return user != null;
        }

        public UserData FindById(string id)
        {
            var user = this._userDataAccess.FindById(id);

            return user != null
                ? new UserData
                {
                    Id = user.Id,
                    Login = user.Login,
                    PasswordHash = user.PasswordHash,
                    UserRoles = this.GetUserRoles(user)
                }
                : null;
        }

        public UserData Find(string login)
        {
            var user = this._userDataAccess.FindByName(login);
            return user != null
                ? new UserData
                {
                    Id = user.Id,
                    Login = user.Login,
                    PasswordHash = user.PasswordHash,
                    UserRoles = this.GetUserRoles(user)
                }
                : null;
        }

        public UserData[] FindByFilter(string searchString)
        {
            return this._userDataAccess.FindByFilter(searchString)
                .Select(user =>
                    new UserData
                    {
                        Id = user.Id,
                        Login = user.Login,
                        PasswordHash = user.PasswordHash,
                        UserRoles = this.GetUserRoles(user)
                    })
                .ToArray();
        }

        public void Create(UserData login)
        {
            uint userRole;
            if (login.UserRoles == null)
            {
                userRole = (uint)UserRole.GenericUser;
            }
            else if (login.UserRoles.Contains(UserRole.System))
            {
                userRole = (uint)UserRole.System;
            } else if (login.UserRoles.Contains(UserRole.Admin))
            {
                userRole = (uint)UserRole.Admin;
            } else
            {
                userRole = (uint)UserRole.GenericUser;
            }
            try
            {
                this._userDataAccess.Create(new User
                {
                    Id = this._hashHelper.GetHashedString(login.Login),
                    Login = login.Login,
                    Role = userRole,
                    PasswordHash = login.PasswordHash
                });
            }
            catch (Exception exp)
            {
                this._logger.Error(exp);
                throw;
            }
        }

        public virtual void Delete(string id)
        {
            var user = this.FindById(id);
            if (user == null) return;
            this._userDataAccess.Delete(id);
        }

        private UserRole[] GetUserRoles(User user)
        {
            var roles = new List<UserRole> { UserRole.GenericUser };
            switch ((UserRole)user.Role)
            {
                case UserRole.Admin:
                    roles.Add(UserRole.Admin);
                    break;
                case UserRole.System:
                    roles.Add(UserRole.System);
                    break;
            }


            return roles.ToArray();
        }
    }
}