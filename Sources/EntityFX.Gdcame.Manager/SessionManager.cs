using System;

using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    using User = EntityFX.Gdcame.DataAccess.Contract.Common.User.User;

    public class SessionManager : EntityFX.Gdcame.Manager.Common.SessionManager
    {

        public SessionManager(ILogger logger, IOperationContextHelper operationContextHelper, IGameSessions gameSessions,
            IUserDataAccessService userDataAccessService)
            : base(logger,operationContextHelper,gameSessions,userDataAccessService)
        {

        }
    }
}