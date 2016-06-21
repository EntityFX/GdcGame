﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public interface IGame
    {
        IDictionary<int, FundsDriver> FundsDrivers { get; }

        FundsCounters FundsCounters { get; }

        int AutomaticStepNumber { get; }

        int ManualStepNumber { get; }

        void Initialize();

        Task<int> PerformAutoStep();

        void PerformManualStep();

        void BuyFundDriver(int fundDriverId);

        void FightAgainstInflation();

        LotteryResult PlayLottery();
    }
}
