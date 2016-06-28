using System;
using System.Web.Http;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public class GameApiController : ApiController
    {
        private readonly IGameManager _game;

        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;

        public GameApiController()
        {
            var simpleUserManagerClient = new SimpleUserManagerClient(string.Empty);
            if (!simpleUserManagerClient.Exists(User.Identity.Name))
            {
                simpleUserManagerClient.Create(User.Identity.Name);
            }

            var sessionManagerClient = new SessionManagerClient(string.Empty);
            var sessionGuid = sessionManagerClient.AddSession(User.Identity.Name);

            _game = new GameManagerClient(string.Empty, sessionGuid);
            _gameDataModelMapper = new GameDataModelMapper();
        }

        [HttpPost]
        [Route()]
        public void PerformManualStep()
        {
            _game.PerformManualStep();
        }
        [HttpPost]
        [Route()]
        public void FightAgainstInflation()
        {
            _game.FightAgainstInflation();
        }

        [HttpGet]
        [Route()]
        public GameDataModel GetGameData(Guid sessionGuid)
        {
            return _gameDataModelMapper.Map(_game.GetGameData(), new GameDataModel());
        }
    }

}
