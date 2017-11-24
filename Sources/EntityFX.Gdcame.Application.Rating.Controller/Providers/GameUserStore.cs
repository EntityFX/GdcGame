using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Application.Api.Common;
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;


    public class GameUserStore : IUserStore<UserIdentity>, IUserPasswordStore<UserIdentity>, IDisposable, IUserEmailStore<UserIdentity>
    {
        private readonly ISimpleUserManager _simpleUserManager;

        public GameUserStore(ISimpleUserManager simpleUserManager)
        {
            this._simpleUserManager = simpleUserManager;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {

        }


        public Task SetEmailAsync(UserIdentity userIdentity, string email)
        {
            throw new NotImplementedException();
        }


        public Task SetEmailConfirmedAsync(UserIdentity userIdentity, bool confirmed)
        {
            throw new NotImplementedException();
        }


        public Task SetEmailAsync(UserIdentity user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetEmailAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName);
        }

        public async Task<bool> GetEmailConfirmedAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(UserIdentity user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserIdentity> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await (Task.Run(() =>
            {
                var res = this._simpleUserManager.Find(normalizedEmail);
                if (res != null)
                {
                    return new UserIdentity
                    {
                        UserName = res.Login,
                        Id = res.Id,
                        PasswordHash = res.PasswordHash
                    };
                }
                return null;
            }, cancellationToken));
        }

        public Task<string> GetNormalizedEmailAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedEmailAsync(UserIdentity user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.UserName = normalizedEmail.ToLowerInvariant();
            return Task.FromResult(true);
        }

        public Task<string> GetUserIdAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(UserIdentity user, string userName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetNormalizedUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(UserIdentity user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName.ToLowerInvariant();
            return Task.FromResult(normalizedName);
        }

        public async Task<IdentityResult> CreateAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            this._simpleUserManager.Create(
                new UserData
                {
                    Login = user.UserName,
                    PasswordHash = user.PasswordHash,
                    UserRoles = new UserRole[] { UserRole.GenericUser }
                });

            return await Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            this._simpleUserManager.Delete(user.Id);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<UserIdentity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var res = this._simpleUserManager.FindById(userId);
                if (res != null)
                {
                    return new UserIdentity
                    {
                        UserName = res.Login,
                        Id = res.Id,
                        PasswordHash = res.PasswordHash
                    };
                }
                return null;
            }, cancellationToken);
        }

        public async Task<UserIdentity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var res = this._simpleUserManager.Find(normalizedUserName.ToLowerInvariant());
                if (res != null)
                {
                    return new UserIdentity
                    {
                        UserName = res.Login,
                        Id = res.Id,
                        PasswordHash = res.PasswordHash,
                        Roles = res.UserRoles.Select(r => r.ToString()).ToArray()
                    };
                }
                return null;
            }, cancellationToken);
        }

        public Task SetPasswordHashAsync(UserIdentity user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException("userIdentity");
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public async Task<string> GetPasswordHashAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException("userIdentity");
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash != null);
        }
    }
}