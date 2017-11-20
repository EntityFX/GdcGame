using System;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Providers.MainServer
{
    public interface IGameDataProvider
    {
        void InitializeGameContext(Guid gameGuid);
        void ClearSession();
        GameDataModel GetGameData();
        CashModel GetCounters();
        BuyItemModel BuyFundDriver(int id);
        ManualStepResultModel PerformManualStep(int? verificationNumber);
        void FightAgainstInflation();
        void ActivateDelayedCounter(int counterId);
        /*UserRating[] GetUsersRatingByCount(int count);
        UserRating FindUserRatingByUserName(string userName);
        UserRating[] FindUserRatingByUserNameAndAroundUsers(string userName, int count);*/
    }
}