using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using Microsoft.AspNet.SignalR;

namespace EntityFX.EconomicsArcade.Application.NotifySignalrService
{
    public class GameDataHub : Hub
    {
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;

        public GameDataHub(IMapper<GameData, GameDataModel> gameDataModelMapper)
        {
            _gameDataModelMapper = gameDataModelMapper;
        }

        public void GetGameData(GameData gameData, UserContext userContext)
        {
            var gameDataModel = _gameDataModelMapper.Map(gameData, null);
            //Clients.All.getGameData(gameDataModel);
            Clients.Caller.getGameData(gameDataModel);
        }
    }
}
