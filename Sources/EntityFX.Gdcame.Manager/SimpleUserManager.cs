using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;

    using User = EntityFX.Gdcame.DataAccess.Contract.Common.User.User;

    public class SimpleUserManager : EntityFX.Gdcame.Manager.Common.SimpleUserManager
    {
        private readonly IHashHelper _hashHelper;
        private readonly ILogger _logger;
        private readonly IUserDataAccessService _userDataAccess;
        private readonly IGameSessions _gameSessions;

        public SimpleUserManager(ILogger logger, IUserDataAccessService userDataAccess, IHashHelper hashHelper, IGameSessions gameSessions) :
            base(logger, userDataAccess, hashHelper)
        {
            _logger = logger;
            _userDataAccess = userDataAccess;
            _hashHelper = hashHelper;
            _gameSessions = gameSessions;
        }

        public virtual void Delete(string id)
        {
            var user = FindById(id);
            if (user == null) return;
            _userDataAccess.Delete(id);
            _gameSessions.RemoveGame(user);
        }

    }
}