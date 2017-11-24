using System.Collections.Generic;
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


    public class GameUserStore : IUserPasswordStore<UserIdentity>, IUserEmailStore<UserIdentity>, IUserRoleStore<UserIdentity>
    {
        private readonly ISimpleUserManager _simpleUserManager;

        public GameUserStore(ISimpleUserManager simpleUserManager)
        {
            this._simpleUserManager = simpleUserManager;
        }

        Task IUserEmailStore<UserIdentity>.SetEmailAsync(UserIdentity user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<string> IUserEmailStore<UserIdentity>.GetEmailAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName);
        }

        async Task<bool> IUserEmailStore<UserIdentity>.GetEmailConfirmedAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }

        Task IUserEmailStore<UserIdentity>.SetEmailConfirmedAsync(UserIdentity user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<UserIdentity> IUserEmailStore<UserIdentity>.FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
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

        Task<string> IUserEmailStore<UserIdentity>.GetNormalizedEmailAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        Task IUserEmailStore<UserIdentity>.SetNormalizedEmailAsync(UserIdentity user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.UserName = normalizedEmail.ToLowerInvariant();
            return Task.FromResult(true);
        }

        Task<string> IUserStore<UserIdentity>.GetUserIdAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        Task<string> IUserStore<UserIdentity>.GetUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        Task IUserStore<UserIdentity>.SetUserNameAsync(UserIdentity user, string userName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        Task<string> IUserStore<UserIdentity>.GetNormalizedUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        Task IUserStore<UserIdentity>.SetNormalizedUserNameAsync(UserIdentity user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName.ToLowerInvariant();
            return Task.FromResult(normalizedName);
        }

        async Task<IdentityResult> IUserStore<UserIdentity>.CreateAsync(UserIdentity user, CancellationToken cancellationToken)
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

        Task<IdentityResult> IUserStore<UserIdentity>.UpdateAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        async Task<IdentityResult> IUserStore<UserIdentity>.DeleteAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            this._simpleUserManager.Delete(user.Id);
            return await Task.FromResult(IdentityResult.Success);
        }

        async Task<UserIdentity> IUserStore<UserIdentity>.FindByIdAsync(string userId, CancellationToken cancellationToken)
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

        async Task<UserIdentity> IUserStore<UserIdentity>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
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

        Task IUserPasswordStore<UserIdentity>.SetPasswordHashAsync(UserIdentity user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException("userIdentity");
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        async Task<string> IUserPasswordStore<UserIdentity>.GetPasswordHashAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException("userIdentity");
            return await Task.FromResult(user.PasswordHash);
        }

        async Task<bool> IUserPasswordStore<UserIdentity>.HasPasswordAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash != null);
        }







        #region IDisposable
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private bool _disposed;
        public void Dispose()
        {
            _disposed = true;
        }
        #endregion

        Task IUserRoleStore<UserIdentity>.AddToRoleAsync(UserIdentity user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IUserRoleStore<UserIdentity>.RemoveFromRoleAsync(UserIdentity user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IList<string>> IUserRoleStore<UserIdentity>.GetRolesAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<string>)user.Roles.ToList());
        }

        Task<bool> IUserRoleStore<UserIdentity>.IsInRoleAsync(UserIdentity user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        Task<IList<UserIdentity>> IUserRoleStore<UserIdentity>.GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}