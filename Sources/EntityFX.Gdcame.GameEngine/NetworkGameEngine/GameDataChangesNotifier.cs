namespace EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine
{
    using System.Collections.Generic;

    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class GameDataChangesNotifier : IGameDataChangesNotifier
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IMapper<Item, Gdcame.Contract.MainServer.Items.Item> _fundsDriverRefreshMapper;
        private readonly IMapper<IGame, GameData> _gameDataRefreshMapper;
        private readonly INotifyConsumerClientFactory _notifyConsumerService;

        public GameDataChangesNotifier(
              IGameDataStoreDataAccessService gameDataStoreDataAccessService
            , IMapperFactory mapperFactory
            , INotifyConsumerClientFactory notifyConsumerService)
        {
            this._mapperFactory = mapperFactory;
            this._gameDataRefreshMapper = this._mapperFactory.Build<IGame, GameData>("GameDataMapper");
            this._fundsDriverRefreshMapper = this._mapperFactory.Build<Item, Gdcame.Contract.MainServer.Items.Item>();

            this._notifyConsumerService = notifyConsumerService;
        }

        public void AutomaticRefreshed(IGame game)
        {
           // var gameData = PrepareGameDataToRefresh(game);
            /*_notifyConsumerService.BuildNotifyConsumerClient()
                .PushGameData(new UserContext {UserId = _userId, UserName = _userName}, gameData);*/
        }

        private GameData PrepareGameDataToRefresh(IGame game)
        {
            var gameData = this._gameDataRefreshMapper.Map(game);

            var fundsDrivers = new List<Gdcame.Contract.MainServer.Items.Item>();
            foreach (var fundDriver in game.Items)
            {
                var fundDriverMapped = this._fundsDriverRefreshMapper.Map(fundDriver);
                fundDriverMapped.IsUnlocked = gameData.Cash.Counters[0].Value >= fundDriverMapped.UnlockValue;
                fundsDrivers.Add(fundDriverMapped);
            }
            gameData.Items = fundsDrivers.ToArray();
            return gameData;
        }
    }
}