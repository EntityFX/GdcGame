using Item = EntityFX.Gdcame.Kernel.Contract.Items.Item;

namespace EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;

    public class GameDataPersister : IGameDataPersister
    {
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IMapper<IGame, StoredGameData> _gameDataPersistMapper;
        private readonly IMapper<Item, StoredItem> _fundsDriverPersistMapper;

        public GameDataPersister(IGameDataStoreDataAccessService gameDataStoreDataAccessService
            , IMapperFactory mapperFactory)
        {
            this._gameDataStoreDataAccessService = gameDataStoreDataAccessService;

            this._mapperFactory = mapperFactory;
            this._gameDataPersistMapper = this._mapperFactory.Build<IGame, StoredGameData>("StoreGameDataMapper");
            this._fundsDriverPersistMapper = this._mapperFactory.Build<Item, StoredItem>();
        }

        public void PersistGamesData(IList<GameWithUserId> gamesWithUserIds)
        {
            var listOfGameDataWithUserId = new StoredGameDataWithUserId[gamesWithUserIds.Count];
            for (int i=0; i<gamesWithUserIds.Count; i++)
            {
                var gameWithUserId = gamesWithUserIds[i];
                var gameData = this.PrepareGameDataToPersist(gameWithUserId.Game);
                listOfGameDataWithUserId[i] = new StoredGameDataWithUserId()
                {
                    StoredGameData = gameData,
                    UserId = gameWithUserId.UserId,
                    CreateDateTime = gameWithUserId.CreateDateTime,
                    UpdateDateTime = DateTime.Now
                };
            }
            this._gameDataStoreDataAccessService.StoreGameDataForUsers(listOfGameDataWithUserId);
        }

        private StoredGameData PrepareGameDataToPersist(IGame game)
        {
            StoredGameData gameData = this._gameDataPersistMapper.Map(game);
            gameData.Items = game.Items.Select(_ => this._fundsDriverPersistMapper.Map(_)).ToArray();
            game.ModifiedFundsDrivers.Clear();
            return gameData;
        }
    }
}