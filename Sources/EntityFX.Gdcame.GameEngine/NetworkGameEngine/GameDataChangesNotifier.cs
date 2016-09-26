﻿using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public class GameDataChangesNotifier : IGameDataChangesNotifier
    {
        private readonly string _userId;
        private readonly string _userName;
        private readonly IMapperFactory _mapperFactory;
        private readonly IMapper<Item, Common.Contract.Items.Item> _fundsDriverRefreshMapper;
        private readonly IMapper<IGame, GameData> _gameDataRefreshMapper;
        private readonly INotifyConsumerClientFactory _notifyConsumerService;

        public GameDataChangesNotifier(string userId
            , string userName
            , IGameDataStoreDataAccessService gameDataStoreDataAccessService///////
            , IMapperFactory mapperFactory
            , INotifyConsumerClientFactory notifyConsumerService)
        {
            _userId = userId;
            _userName = userName;

            _mapperFactory = mapperFactory;
            _gameDataRefreshMapper = _mapperFactory.Build<IGame, GameData>("GameDataMapper");
            _fundsDriverRefreshMapper = _mapperFactory.Build<Item, Common.Contract.Items.Item>();

            _notifyConsumerService = notifyConsumerService;
        }

        public void AutomaticRefreshed(IGame game)
        {
            var gameData = PrepareGameDataToRefresh(game);
            _notifyConsumerService.BuildNotifyConsumerClient()
                .PushGameData(new UserContext {UserId = _userId, UserName = _userName}, gameData);
        }

        private GameData PrepareGameDataToRefresh(IGame game)
        {
            var gameData = _gameDataRefreshMapper.Map(game);

            var fundsDrivers = new List<Common.Contract.Items.Item>();
            foreach (var fundDriver in game.Items)
            {
                var fundDriverMapped = _fundsDriverRefreshMapper.Map(fundDriver.Value);
                fundDriverMapped.IsUnlocked = gameData.Cash.Counters[0].Value >= fundDriverMapped.UnlockBalance;
                fundsDrivers.Add(fundDriverMapped);
            }
            gameData.Items = fundsDrivers.ToArray();
            return gameData;
        }
    }
}