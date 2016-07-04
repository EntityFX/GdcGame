using System.Web.Http.Controllers;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Providers;
using Microsoft.AspNet.SignalR;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Hubs
{
    public class GameDataHub : Hub
    {
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;

        public GameDataHub(IMapper<GameData, GameDataModel> gameDataModelMapper)
        {
            _gameDataModelMapper = gameDataModelMapper;
        }

        public void GetGameData(GameData gameData)
        {
            var gameDataModel = _gameDataModelMapper.Map(gameData, null);
            //Clients.
            Clients.All.getGameData(gameDataModel);
        }
    }
}