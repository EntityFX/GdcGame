using System;
using EntityFX.EconomicsArcade.Presentation.Models;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public interface IGameApiController
    {

        VerificationNumberModel PerformManualStep();

        bool FightAgainstInflation();

        bool ActivateDelayedCounter();

        GameDataModel GetGameData();

        FundsCounterModel GetCounters();

        bool BuyFundDriver(int id);
    }
}