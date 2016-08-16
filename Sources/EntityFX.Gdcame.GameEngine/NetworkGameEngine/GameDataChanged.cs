using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Item = EntityFX.Gdcame.GameEngine.Contract.Items.Item;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public class NotifyGameDataChanged : INotifyGameDataChanged
    {
        private readonly int _userId;
        private readonly string _userName;
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;
        private readonly IMapper<IGame, StoredGameData> _gameDataPersistMapper;
        private readonly IMapper<IGame, GameData> _gameDataRefreshMapper;
        private readonly IMapper<Item, StoredItem> _fundsDriverPersistMapper;
        private readonly IMapper<Item, Common.Contract.Items.Item> _fundsDriverRefreshMapper;
        private readonly INotifyConsumerClientFactory _notifyConsumerService;

        public NotifyGameDataChanged(int userId
            , string userName
            , IGameDataStoreDataAccessService gameDataStoreDataAccessService
            , IMapper<IGame, StoredGameData> gameDataPersistMapper
            , IMapper<IGame, GameData> gameDataRefreshMapper
            , IMapper<Item, StoredItem> fundsDriverPersistMapper
            , IMapper<Item, Common.Contract.Items.Item> fundsDriverRefreshMapper
            , INotifyConsumerClientFactory notifyConsumerService)
        {
            _userId = userId;
            _userName = userName;
            _gameDataStoreDataAccessService = gameDataStoreDataAccessService;
            _gameDataPersistMapper = gameDataPersistMapper;
            _gameDataRefreshMapper = gameDataRefreshMapper;
            _fundsDriverPersistMapper = fundsDriverPersistMapper;
            _fundsDriverRefreshMapper = fundsDriverRefreshMapper;
            _notifyConsumerService = notifyConsumerService;
        }

        public void GameDataChanged(IGame game)
        {
            var gameData = PrepareGameDataToPersist(game);
            _gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }

        public void AutomaticRefreshed(IGame game)
        {
            var gameData = PrepareGameDataToRefresh(game);
            _notifyConsumerService.BuildNotifyConsumerClient().PushGameData(new UserContext() { UserId = _userId, UserName = _userName }, gameData);
        }

        public void FundsDriverBought(IGame game, Item item)
        {
            //var gameData = PrepareGameDataToPersist(game);
            //_gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }

        private StoredGameData PrepareGameDataToPersist(IGame game)
        {
            var gameData = _gameDataPersistMapper.Map(game);
            gameData.Items = game.Items.Values.Select(_ => _fundsDriverPersistMapper.Map(_)).ToArray();
            game.ModifiedFundsDrivers.Clear();
            return gameData;
        }

        private GameData PrepareGameDataToRefresh(IGame game)
        {
            var gameData = _gameDataRefreshMapper.Map(game);

            var fundsDrivers = new List<Common.Contract.Items.Item>();
            foreach (var fundDriver in game.Items)
            {
                var fundDriverMapped = _fundsDriverRefreshMapper.Map(fundDriver.Value);
                fundDriverMapped.IsActive = gameData.Cash.Counters[0].Value >= fundDriverMapped.UnlockValue;
                fundsDrivers.Add(fundDriverMapped);
            }
            gameData.Items = fundsDrivers.ToArray();
            return gameData;
        }

    }
}