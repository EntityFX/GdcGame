using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Contract.Model;

namespace EntityFX.Gdcame.Presentation.Contract.Controller
{
    public interface IGameApiController
    {
        Task<ManualStepResultModel> PerformManualStepAsync(int? id);

        Task<bool> FightAgainstInflationAsync();

        Task<bool> ActivateDelayedCounterAsync(int counterId);

        Task<GameDataModel> GetGameDataAsync();

        Task<CashModel> GetCountersAsync();

        Task<BuyDriverModel> BuyFundDriverAsync(int id);
    }
}