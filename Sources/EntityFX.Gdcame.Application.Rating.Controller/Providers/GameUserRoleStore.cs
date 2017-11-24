using System;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.Manager.Contract.Common.UserManager;
using Microsoft.AspNetCore.Identity;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    public class GameUserRoleStore : IRoleStore<UserIdentityRole>
    {
        private readonly ISimpleUserManager _simpleUserManager;

        public GameUserRoleStore(ISimpleUserManager simpleUserManager)
        {
            _simpleUserManager = simpleUserManager;
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

        public Task<IdentityResult> CreateAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<string> GetRoleIdAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(UserIdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(UserIdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return SetRoleNameAsync(role, normalizedName, cancellationToken);
        }

        public async Task<UserIdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await (Task.Run(() =>
            {
                var res = this._simpleUserManager.FindById(roleId);
                if (res != null)
                {
                    return new UserIdentityRole
                    {
                        Name = res.Login,
                        Id = res.Id
                    };
                }
                return null;
            }, cancellationToken));
        }

        public async Task<UserIdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await (Task.Run(() =>
            {
                var res = this._simpleUserManager.Find(normalizedRoleName);
                if (res != null)
                {
                    return new UserIdentityRole
                    {
                        Name = res.Login,
                        Id = res.Id
                    };
                }
                return null;
            }, cancellationToken));
        }
    }
}