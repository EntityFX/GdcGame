using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicArcade.Engine.GameEngine.UssrSimulator
{
    public  class NotifyGameDataChanged : INotifyGameDataChanged
    {
        private readonly int _userId;
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;

        public NotifyGameDataChanged(int userId, IGameDataStoreDataAccessService gameDataStoreDataAccessService)
        {
            _userId = userId;
            _gameDataStoreDataAccessService = gameDataStoreDataAccessService;
        }

        public void GameDataChanged(GameData gameData)
        {
            _gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }
    }
}