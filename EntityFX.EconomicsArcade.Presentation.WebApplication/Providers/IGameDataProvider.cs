using System;
using EntityFX.EconomicsArcade.Presentation.Models;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Providers
{
    public interface IGameDataProvider
    {
        Guid GameGuid { get; }
        void Initialize(string userName);
        GameDataModel GetGameData();
        FundsCounterModel GetCounters();
        void BuyFundDriver(int id);
        void PerformManualStep();
        void FightAgainstInflation();
    }
}