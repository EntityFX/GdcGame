using System;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Web.Model;

namespace EntityFX.Gdcame.Presentation.Web.Providers.Providers
{
    public interface IGameDataProvider
    {
        void InitializeSession(string userName);
        void InitializeGameContext(Guid gameGuid);
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