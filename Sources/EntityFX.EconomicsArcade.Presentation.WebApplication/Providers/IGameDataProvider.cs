using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.UserRating;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Model.Common.Model;
using EntityFX.EconomicsArcade.Presentation.Models;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Providers
{
    public interface IGameDataProvider
    {
        void InitializeSession(string userName);
        void ClearSession();
        GameDataModel GetGameData();
        FundsCounterModel GetCounters();
        BuyDriverModel BuyFundDriver(int id);
        ManualStepResultModel PerformManualStep(int? verificationNumber);
        void FightAgainstInflation();
        void ActivateDelayedCounter(int counterId);
        UserRating[] GetUsersRatingByCount(int count);
        UserRating FindUserRatingByUserName(string userName);
        UserRating[] FindUserRatingByUserNameAndAroundUsers(string userName, int count);
    }
}