namespace EntityFX.Gdcame.Utils.MainServer
{
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;
    using System;

    public class GameFactory : IGameFactory
    {
        private readonly IIocContainer _unityContainer;

        public GameFactory(IIocContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IGame BuildGame(string userId, string userName)
        {
            var game = _unityContainer.Resolve<IGame>(null,
                new Tuple<string, object>("gameDataChangesNotifier", _unityContainer.Resolve<IGameDataChangesNotifier>(null,
                    new Tuple<string, object>("userId", userId), new Tuple<string, object>("userName", userName))),
                new Tuple<string, object>("userId", userId));
            return game;
        }
    }
}