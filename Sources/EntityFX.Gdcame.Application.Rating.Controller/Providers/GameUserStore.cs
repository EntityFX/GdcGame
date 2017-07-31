namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Application.Api.Common;
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;

    using Microsoft.AspNet.Identity;

    public class GameUserStore : IUserStore<UserIdentity>, IUserPasswordStore<UserIdentity>, IDisposable , IUserEmailStore<UserIdentity>
    {
        private readonly ISimpleUserManager _simpleUserManager;

        public GameUserStore(ISimpleUserManager simpleUserManager)
        {
            this._simpleUserManager = simpleUserManager;
        }

        /// <summary>
        ///     Set the password hash for a userIdentity
        /// </summary>
        /// <param name="userIdentity" />
        /// <param name="passwordHash" />
        /// <returns />
        public virtual Task SetPasswordHashAsync(UserIdentity userIdentity, string passwordHash)
        {
            if (userIdentity == null)
                throw new ArgumentNullException("userIdentity");
            userIdentity.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        /// <summary>
        ///     Get the password hash for a userIdentity
        /// </summary>
        /// <param name="userIdentity" />
        /// <returns />
        public virtual async Task<string> GetPasswordHashAsync(UserIdentity userIdentity)
        {
            if (userIdentity == null)
                throw new ArgumentNullException("userIdentity");
            return await Task.FromResult(userIdentity.PasswordHash);
        }

        /// <summary>
        ///     Returns true if the userIdentity has a password set
        /// </summary>
        /// <param name="userIdentity" />
        /// <returns />
        public virtual async Task<bool> HasPasswordAsync(UserIdentity userIdentity)
        {
            return await Task.FromResult(userIdentity.PasswordHash != null);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {

        }

        public async Task CreateAsync(UserIdentity userIdentity)
        {

            await Task.Run(
                () =>
                    this._simpleUserManager.Create(new UserData { Login = userIdentity.UserName, PasswordHash = userIdentity.PasswordHash, UserRoles = new UserRole[] {UserRole.GenericUser}}));
        }

        public Task UpdateAsync(UserIdentity userIdentity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(UserIdentity userIdentity)
        {
            await (new TaskFactory()).StartNew(() => this._simpleUserManager.Delete(userIdentity.Id));
        }

        public async Task<UserIdentity> FindByIdAsync(string userId)
        {
            return await Task.Run(() =>
            {
                var res = this._simpleUserManager.FindById(userId);
                if (res != null)
                {
                    return new UserIdentity
                    {
                        UserName = res.Login,
                        Id = res.Id.ToString(CultureInfo.InvariantCulture),
                        PasswordHash = res.PasswordHash
                    };
                }
                return null;
            });
        }

        public async Task<UserIdentity> FindByNameAsync(string userName)
        {
            return await Task.Run(() =>
            {
                var res = this._simpleUserManager.Find(userName);
                if (res != null)
                {
                    return new UserIdentity
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

        public Task SetEmailAsync(UserIdentity userIdentity, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetEmailAsync(UserIdentity userIdentity)
        {
            return await Task.FromResult(userIdentity.UserName);
        }

        public async Task<bool> GetEmailConfirmedAsync(UserIdentity userIdentity)
        {
            return await Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(UserIdentity userIdentity, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public async Task<UserIdentity> FindByEmailAsync(string email)
        {
            return await (new TaskFactory()).StartNew(() =>
            {
                var res = this._simpleUserManager.Find(email);
                if (res != null)
                {
                    return new UserIdentity
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