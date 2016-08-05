using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Web.Model;

namespace EntityFX.Gdcame.Presentation.Web.Controller
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