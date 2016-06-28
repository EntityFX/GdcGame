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
            Guid ses;
            using (var proxy = new SessionManagerProxy(Guid.Empty))
            {
                var channel = proxy.CreateChannel(new Uri("net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.SessionManager.ISessionManager"));
                ses = channel.AddSession(User.Identity.Name);
                proxy.CloseChannel();
            }
            _game = new GameManagerClient(ses);
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
