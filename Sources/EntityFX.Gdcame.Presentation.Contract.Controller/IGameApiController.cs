using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Contract.Controller.MainServer
{
    public interface IGameApiController
    {
        Task<ManualStepResultModel> PerformManualStepAsync(int? id);

        Task<CashModel> FightAgainstInflationAsync();

        Task<CashModel> ActivateDelayedCounterAsync(int counterId);

        Task<GameDataModel> GetGameDataAsync();

        Task<CashModel> GetCountersAsync();

        Task<BuyItemModel> BuyFundDriverAsync(int id);
    }
}