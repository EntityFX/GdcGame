namespace EntityFX.Gdcame.Engine.Contract.GameEngine
{
    using System;
    using System.Collections.Generic;

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;
    using EntityFX.Gdcame.Engine.Contract.Common;
    using EntityFX.Gdcame.Kernel.Contract;

    public interface IGameSessions : ISessions
    {
        event EventHandler AllGamesRemoved;
        event EventHandler<Tuple<string, string>> GameRemoved;
        event EventHandler<Tuple<string, string>> GameStarted;

        GamePerformanceInfo PerformanceInfo { get; }

        IGame StartGame(string userId, string login);
        IGame GetGame(string username);
        IGame GetGame(Guid sessionId);
        UserGameSessionStatus GetGameSessionStatus(string username);

        void RemoveAllGames();

        void RemoveGame(UserData user);

        IDictionary<string, IGame> Games { get; }
    }
}