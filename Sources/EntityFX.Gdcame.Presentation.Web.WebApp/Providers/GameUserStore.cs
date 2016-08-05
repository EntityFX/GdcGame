using System;
using System.Globalization;
using System.Threading.Tasks;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.Presentation.Web.WebApp.Models;
using Microsoft.AspNet.Identity;

namespace EntityFX.Gdcame.Presentation.Web.WebApp.Providers
{
    public class GameUserStore : IUserStore<GameUser>, IUserPasswordStore<GameUser>, IUserEmailStore<GameUser>
    {
        private readonly ISimpleUserManager _simpleUserManager;

        public GameUserStore(ISimpleUserManager simpleUserManager)
        {
            _simpleUserManager = simpleUserManager;
        }

        public void Dispose()
        {

        }

        public Task CreateAsync(GameUser user)
        {
            return (new TaskFactory()).StartNew(() => _simpleUserManager.Create(new UserData(){Login = user.UserName, PasswordHash = user.PasswordHash}));
        }

        public Task UpdateAsync(GameUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(GameUser user)
        {
            return (new TaskFactory()).StartNew(() => _simpleUserManager.Delete(int.Parse(user.Id)));
        }

        public Task<GameUser> FindByIdAsync(string userId)
        {
            return (new TaskFactory()).StartNew(() =>
            {
                var res = _simpleUserManager.FindById(int.Parse(userId));
                if (res != null)
                {
                    return new GameUser()
                    {
                       UserName = res.Login,
                       Id = res.Id.ToString(CultureInfo.InvariantCulture),
                       PasswordHash = res.PasswordHash
                    };
                }
                return null;
            });
        }

        public Task<GameUser> FindByNameAsync(string userName)
        {
            return (new TaskFactory()).StartNew(() =>
            {
                var res = _simpleUserManager.Find(userName);
                if (res != null)
                {
                    return new GameUser()
                    {
                        UserName = res.Login,
                        Id = res.Id.ToString(CultureInfo.InvariantCulture),
                        PasswordHash = res.PasswordHash
                    };
                }
                return null;
            });
        }

        /// <summary>
        /// Set the password hash for a user
        /// 
        /// </summary>
        /// <param name="user"/><param name="passwordHash"/>
        /// <returns/>
        public virtual Task SetPasswordHashAsync(GameUser user, string passwordHash)
        {
            if ((object)user == null)
                throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            return (Task)Task.FromResult<int>(0);
        }

        /// <summary>
        /// Get the password hash for a user
        /// 
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public virtual Task<string> GetPasswordHashAsync(GameUser user)
        {
            if ((object)user == null)
                throw new ArgumentNullException("user");
            else
                return Task.FromResult<string>(user.PasswordHash);
        }

        /// <summary>
        /// Returns true if the user has a password set
        /// 
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        public virtual Task<bool> HasPasswordAsync(GameUser user)
        {
            return Task.FromResult<bool>(user.PasswordHash != null);
        }

        public Task SetEmailAsync(GameUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(GameUser user)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> GetEmailConfirmedAsync(GameUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(GameUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<GameUser> FindByEmailAsync(string email)
        {
            return (new TaskFactory()).StartNew(() =>
            {
                var res = _simpleUserManager.Find(email);
                if (res != null)
                {
                    return new GameUser()
                    {
                        UserName = res.Login,
                        Id = res.Id.ToString(CultureInfo.InvariantCulture),
                        PasswordHash = res.PasswordHash
                    };
                }
                return null;
            });
        }
    }
}