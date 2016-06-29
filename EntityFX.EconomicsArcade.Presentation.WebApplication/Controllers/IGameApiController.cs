using System;
using EntityFX.EconomicsArcade.Presentation.Models;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public interface IGameApiController
    {

        bool PerformManualStep();

        bool FightAgainstInflation();

        bool ActivateDelayedCounter();

        GameDataModel GetGameData(Guid? id);

        bool BuyFundDriver(int id);
    }
}