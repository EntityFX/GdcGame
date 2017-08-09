namespace EntityFX.Gdcame.Engine.GameEngine
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;
    using EntityFX.Gdcame.Engine.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;

    public class GameSessions : SessionsProvider, IGameSessions
    {
        private readonly IGameFactory _gameFactory;

        private readonly ILogger _logger;

        private readonly ConcurrentDictionary<string, IGame> _userGamesStorage =
            new ConcurrentDictionary<string, IGame>();


        private readonly GamePerformanceInfo _performanceInfo;

        public GameSessions(ILogger logger, IGameFactory gameFactory, GamePerformanceInfo performanceInfo)
            : base(logger)
        {
            this._logger = logger;
            this._gameFactory = gameFactory;
            this._performanceInfo = performanceInfo;
        }



        public IDictionary<string, IGame> Games
        {
            get { return this._userGamesStorage; }
        }



        private static readonly object _stdLock = new { };

        public IGame StartGame(string userId, string login)
        {
            var game = this.BuildGame(userId, login);
            game.Initialize();
            if (this.GameStarted != null)
            {
                this.GameStarted(this, new Tuple<string, string>(login, userId));
            }
            return game;
        }
        //TODO: add FreezeUserGame(userId, login) that will set IsFreezed property of Game that will be checked by Game methods.
        public IGame GetGame(Guid sessionId)
        {
            Session session;

            session = this.GetSession(sessionId);
            return GetGame(session.Login);
        }

        public IGame GetGame(string username)
        {
            IGame game;
            if (!this._userGamesStorage.TryGetValue(username, out game))
            {
                this._logger.Warning("Cannot get game for user {0}", username);
            }
            return game;
        }

        public UserGameSessionStatus GetGameSessionStatus(string username)
        {
            if (this.Sessions.Values.Count(_ => _.Login == username) > 0)
            {
                return UserGameSessionStatus.Online;
            }
            if (!this._userGamesStorage.ContainsKey(username))
            {
                return UserGameSessionStatus.GameNotStarted;
            }
            return UserGameSessionStatus.Offline;
        }

        public override Guid AddSession(User user)
        {
            if (user.Role == (uint)UserRole.System) return base.AddSession(user);
            if (!this._userGamesStorage.ContainsKey(user.Login))
            {
                var game = this.StartGame(user.Id, user.Login);
                if (!this._userGamesStorage.TryAdd(user.Login, game))
                {
                    this._logger.Warning("Cannot add game for user {0}", user.Login);
                }
            }
            return base.AddSession(user);
        }


        public void RemoveGame(UserData user)
        {

            var userSessions = this.Sessions.Values.Where(_ => _.Login == user.Login);
            foreach (var userSession in userSessions)
            {
                this.RemoveSession(userSession.SessionIdentifier);
            }

            IGame game;
            if (!this._userGamesStorage.TryRemove(user.Login, out game))
            {
                this._logger.Warning("Cannot remove game for user {0}", user.Login);
            }
            if (this.GameStarted != null)
            {
                this.GameRemoved(this, new Tuple<string, string>(user.Login, user.Id));
            }
        }

        public void RemoveAllGames()
        {
            this.RemoveAllSessions();
            this._userGamesStorage.Clear();
            if (this.AllGamesRemoved != null)
            {
                this.AllGamesRemoved(this, null);
            }
        }



        public GamePerformanceInfo PerformanceInfo
        {
            get { return this._performanceInfo; }
        }

        public event EventHandler<Tuple<string, string>> GameStarted;

        public event EventHandler<Tuple<string, string>> GameRemoved;

        public event EventHandler AllGamesRemoved;

        private IGame BuildGame(string userId, string userName)
        {
            return this._gameFactory.BuildGame(userId, userName);
        }

    }
}