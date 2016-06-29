using System;
using System.Web.Http;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Presentation.WebApplication.App_Start;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Factories;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Providers;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public class GameApiController : ApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        public GameApiController()
        {
            _gameDataProvider = new GameDataProvider(
                new GameClientFactory(UnityConfig.GetConfiguredContainer()),
                UnityConfig.GetConfiguredContainer().Resolve<ISimpleUserManager>(),
                UnityConfig.GetConfiguredContainer().Resolve<SessionManagerClient>(),
                UnityConfig.GetConfiguredContainer().Resolve<IMapper<GameData, GameDataModel>>()
                );
        }

        [HttpPost]
        public void PerformManualStep()
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            _gameDataProvider.PerformManualStep();
        }
        [HttpPost]
        public void FightAgainstInflation()
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            _gameDataProvider.FightAgainstInflation();
        }

        [HttpGet]
        public GameDataModel GetGameData(Guid? id)
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            return _gameDataProvider.GetGameData();
        }

        [HttpPost]
        public void BuyFundDriver([FromUri]int id)
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            _gameDataProvider.BuyFundDriver(id);
        }
    }

}
