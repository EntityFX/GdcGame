using System.Diagnostics.Contracts;
using EntityFX.EconomicsArcade.Application.NotifyConsumerService;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicArcade.Engine.GameEngine.NetworkGameEngine
{
    public class NotifyGameDataChanged : INotifyGameDataChanged
    {
        private readonly int _userId;
        private readonly string _userName;
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;
        private readonly IMapper<IGame, GameData> _gameDataMapper;
        private readonly IMapper<FundsDriver, EconomicsArcade.Contract.Common.Funds.FundsDriver> _fundsDriverMapper;
        private readonly INotifyConsumerService _notifyConsumerService;

        public NotifyGameDataChanged(int userId
            , string userName
            , IGameDataStoreDataAccessService gameDataStoreDataAccessService
            , IMapper<IGame, GameData> gameDataMapper
            , IMapper<FundsDriver, EconomicsArcade.Contract.Common.Funds.FundsDriver> fundsDriverMapper
            , INotifyConsumerService notifyConsumerService)
        {
            _userId = userId;
            _userName = userName;
            _gameDataStoreDataAccessService = gameDataStoreDataAccessService;
            _gameDataMapper = gameDataMapper;
            _fundsDriverMapper = fundsDriverMapper;
            _notifyConsumerService = notifyConsumerService;
        }

        public void GameDataChanged(IGame game)
        {
            var gameData = PrepareGameDataToPersist(game);
            _gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }

        public void AutomaticRefreshed(IGame game)
        {
            var gameData = PrepareGameDataToPersist(game);
            _notifyConsumerService.PushGameData(new UserContext() { UserId = _userId, UserName = _userName }, gameData);
        }

        public void FundsDriverBought(IGame game, FundsDriver fundsDriver)
        {
            var gameData = PrepareGameDataToPersist(game, fundsDriver);
            _gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }

        private GameData PrepareGameDataToPersist(IGame game, FundsDriver fundDriver = null)
        {
            var gameData = _gameDataMapper.Map(game);
            gameData.FundsDrivers = fundDriver != null
                ? new[] { _fundsDriverMapper.Map(fundDriver) }
                : new EconomicsArcade.Contract.Common.Funds.FundsDriver[] { };
            return gameData;
        }

    }
}