using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Api.MainServer.Models;
using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.UserManager;
using Microsoft.AspNet.Identity;

namespace EntityFX.Gdcame.Application.Api.MainServer.Providers
{
    public class GameUserStore : IUserStore<GameUser>, IUserPasswordStore<GameUser>, IDisposable , IUserEmailStore<GameUser>
    {
        private readonly ISimpleUserManager _simpleUserManager;

        public GameUserStore(ISimpleUserManager simpleUserManager)
        {
            _simpleUserManager = simpleUserManager;
        }

        /// <summary>
        ///     Set the password hash for a user
        /// </summary>
        /// <param name="user" />
        /// <param name="passwordHash" />
        /// <returns />
        public virtual Task SetPasswordHashAsync(GameUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        /// <summary>
        ///     Get the password hash for a user
        /// </summary>
        /// <param name="user" />
        /// <returns />
        public virtual async Task<string> GetPasswordHashAsync(GameUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return await Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        ///     Returns true if the user has a password set
        /// </summary>
        /// <param name="user" />
        /// <returns />
        public virtual async Task<bool> HasPasswordAsync(GameUser user)
        {
            return await Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {

        }

        public async Task CreateAsync(GameUser user)
        {

            await Task.Run(
                () =>
                    _simpleUserManager.Create(new UserData { Login = user.UserName, PasswordHash = user.PasswordHash, UserRoles = new UserRole[] {UserRole.GenericUser}}));
        }

        public Task UpdateAsync(GameUser user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(GameUser user)
        {
            await (new TaskFactory()).StartNew(() => _simpleUserManager.Delete(user.Id));
        }

        public async Task<GameUser> FindByIdAsync(string userId)
        {
            return await Task.Run(() =>
            {
                var res = _simpleUserManager.FindById(userId);
                if (res != null)
                {
                    return new GameUser
                    {
                        UserName = res.Login,
                        Id = res.Id.ToString(CultureInfo.InvariantCulture),
                        PasswordHash = res.PasswordHash
                    };
                }
                return null;
            });
        }

        public async Task<GameUser> FindByNameAsync(string userName)
        {
            return await Task.Run(() =>
            {
                var res = _simpleUserManager.Find(userName);
                if (res != null)
                {
                    return new GameUser
                    {
                        UserName = res.Login,
                        Id = res.Id.ToString(CultureInfo.InvariantCulture),
                        PasswordHash = res.PasswordHash,
                        Roles = res.UserRoles.Select(r => r.ToString()).ToArray()
                    };
                }
                return null;
            });
        }

        public Task SetEmailAsync(GameUser user, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetEmailAsync(GameUser user)
        {
            return await Task.FromResult(user.UserName);
        }

        public async Task<bool> GetEmailConfirmedAsync(GameUser user)
        {
            return await Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(GameUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public async Task<GameUser> FindByEmailAsync(string email)
        {
            return await (new TaskFactory()).StartNew(() =>
            {
                var res = _simpleUserManager.Find(email);
                if (res != null)
                {
                    return new GameUser
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