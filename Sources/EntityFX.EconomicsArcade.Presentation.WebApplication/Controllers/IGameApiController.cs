using System;
using EntityFX.EconomicsArcade.Presentation.Models;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public interface IGameApiController
    {

        ManualStepResultModel PerformManualStep();

        bool FightAgainstInflation();

        bool ActivateDelayedCounter();

        GameDataModel GetGameData();

        FundsCounterModel GetCounters();

        BuyDriverModel BuyFundDriver(int id);
    }
}