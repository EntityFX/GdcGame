﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.Application.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace EntityFX.Gdcame.Presentation.Web.Api.Providers
{
    public class GameUserStore : IUserStore<GameUser>, IUserPasswordStore<GameUser>, IDisposable /*, IUserEmailStore<GameUser>*/
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
        public virtual Task<string> GetPasswordHashAsync(GameUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        ///     Returns true if the user has a password set
        /// </summary>
        /// <param name="user" />
        /// <returns />
        public virtual Task<bool> HasPasswordAsync(GameUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {

        }

        public Task CreateAsync(GameUser user)
        {
            return
                Task.Run(
                    () =>
                        _simpleUserManager.Create(new UserData {Login = user.UserName, PasswordHash = user.PasswordHash}));
        }

        public Task UpdateAsync(GameUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(GameUser user)
        {
            return (new TaskFactory()).StartNew(() => _simpleUserManager.Delete(user.Id));
        }

        public Task<GameUser> FindByIdAsync(string userId)
        {
            return Task.Run(() =>
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

        public Task<GameUser> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
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