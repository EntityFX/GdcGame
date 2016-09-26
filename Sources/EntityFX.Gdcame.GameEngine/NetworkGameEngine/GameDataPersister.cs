using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.Infrastructure.Common;
using Item = EntityFX.Gdcame.GameEngine.Contract.Items.Item;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public class GameDataPersister : IGameDataPersister
    {
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IMapper<IGame, StoredGameData> _gameDataPersistMapper;
        private readonly IMapper<Item, StoredItem> _fundsDriverPersistMapper;

        public GameDataPersister(IGameDataStoreDataAccessService gameDataStoreDataAccessService
            , IMapperFactory mapperFactory)
        {
            _gameDataStoreDataAccessService = gameDataStoreDataAccessService;

            _mapperFactory = mapperFactory;
            _gameDataPersistMapper = _mapperFactory.Build<IGame, StoredGameData>("StoreGameDataMapper");
            _fundsDriverPersistMapper = _mapperFactory.Build<Item, StoredItem>();
        }

        public void PersistGamesData(IList<GameWithUserId> gamesWithUserIds)
        {
            var listOfGameDataWithUserId = new StoredGameDataWithUserId[gamesWithUserIds.Count];
            for (int i=0; i<gamesWithUserIds.Count; i++)
            {
                var gameWithUserId = gamesWithUserIds[i];
                var gameData = PrepareGameDataToPersist(gameWithUserId.Game);
                listOfGameDataWithUserId[i] = new StoredGameDataWithUserId()
                {
                    StoredGameData = gameData,
                    UserId = gameWithUserId.UserId
                };
            }
            _gameDataStoreDataAccessService.StoreGameDataForUsers(listOfGameDataWithUserId);
        }

        private StoredGameData PrepareGameDataToPersist(IGame game)
        {
            StoredGameData gameData = _gameDataPersistMapper.Map(game);
            gameData.Items = game.Items.Values.Select(_ => _fundsDriverPersistMapper.Map(_)).ToArray();
            game.ModifiedFundsDrivers.Clear();
            return gameData;
        }
    }
}