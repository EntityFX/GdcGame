using EntityFX.EconomicsArcade.Model.Common.Model;
using EntityFX.EconomicsArcade.Presentation.Models;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public interface IGameApiController
    {

        ManualStepResultModel PerformManualStep(int? id);

        bool FightAgainstInflation();

        bool ActivateDelayedCounter(int counterId);

        GameDataModel GetGameData();

        FundsCounterModel GetCounters();

        BuyDriverModel BuyFundDriver(int id);
    }
}