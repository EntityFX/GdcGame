using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using FundsDriver = EntityFX.Gdcame.GameEngine.Contract.Funds.FundsDriver;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public class NotifyGameDataChanged : INotifyGameDataChanged
    {
        private readonly int _userId;
        private readonly string _userName;
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;
        private readonly IMapper<IGame, StoreGameData> _gameDataPersistMapper;
        private readonly IMapper<IGame, GameData> _gameDataRefreshMapper;
        private readonly IMapper<FundsDriver, StoreFundsDriver> _fundsDriverPersistMapper;
        private readonly IMapper<FundsDriver, Gdcame.Common.Contract.Funds.FundsDriver> _fundsDriverRefreshMapper;
        private readonly INotifyConsumerClientFactory _notifyConsumerService;

        public NotifyGameDataChanged(int userId
            , string userName
            , IGameDataStoreDataAccessService gameDataStoreDataAccessService
            , IMapper<IGame, StoreGameData> gameDataPersistMapper
            , IMapper<IGame, GameData> gameDataRefreshMapper
            , IMapper<FundsDriver, StoreFundsDriver> fundsDriverPersistMapper
            , IMapper<FundsDriver, Gdcame.Common.Contract.Funds.FundsDriver> fundsDriverRefreshMapper
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

        public void FundsDriverBought(IGame game, FundsDriver fundsDriver)
        {
            //var gameData = PrepareGameDataToPersist(game);
            //_gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }

        private StoreGameData PrepareGameDataToPersist(IGame game)
        {
            var gameData = _gameDataPersistMapper.Map(game);
            gameData.FundsDrivers = game.FundsDrivers.Values.Select(_ => _fundsDriverPersistMapper.Map(_)).ToArray();
            game.ModifiedFundsDrivers.Clear();
            return gameData;
        }

        private GameData PrepareGameDataToRefresh(IGame game)
        {
            var gameData = _gameDataRefreshMapper.Map(game);

            var fundsDrivers = new List<Gdcame.Common.Contract.Funds.FundsDriver>();
            foreach (var fundDriver in game.FundsDrivers)
            {
                var fundDriverMapped = _fundsDriverRefreshMapper.Map(fundDriver.Value);
                fundDriverMapped.IsActive = gameData.Counters.Counters[0].Value >= fundDriverMapped.UnlockValue;
                fundsDrivers.Add(fundDriverMapped);
            }
            gameData.FundsDrivers = fundsDrivers.ToArray();
            return gameData;
        }

    }
}