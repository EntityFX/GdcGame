using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Engine.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.UssrSimulator
{
    public class UssrSimulatorGame : GameBase
    {
        private readonly IGameDataRepository _gameDataRepository;

        private GameData _gameData;

        public UssrSimulatorGame(IGameDataRepository gameDataRepository)
        {
            _gameDataRepository = gameDataRepository;
        }

        public decimal Communism
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.Communism].SubValue;
            }
        }

        public decimal Production
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.Production].SubValue;
            }
        }

        public decimal Taxes
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.Tax].SubValue;
            }
        }

        public decimal FiveYearPlan
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.FiveYearPlan].SubValue;
            }
        }

        protected override IDictionary<int, FundsDriver> GetFundsDrivers()
        {
            var result = new Dictionary<int, FundsDriver>();
            foreach (var fundDriver in _gameData.FundsDrivers)
            {
                var incrementors = new Dictionary<int, IncrementorBase>();
                foreach (var incrementor in fundDriver.Incrementors)
                {
                    IncrementorBase value;
                    if (incrementor.Value.IncrementorType == EntityFX.EconomicsArcade.Contract.Common.Incrementors.IncrementorTypeEnum.ValueIncrementor)
                    {
                        value = new ValueIncrementor(incrementor.Value.Value);
                    }
                    else if (incrementor.Value.IncrementorType == EntityFX.EconomicsArcade.Contract.Common.Incrementors.IncrementorTypeEnum.PercentageIncrementor)
                    {
                        value = new PercentageIncrementor(incrementor.Value.Value);
                    }
                    else
                    {
                        value = null;
                    }
                    incrementors.Add(incrementor.Key, value);
                }

                result.Add(fundDriver.Id, new FundsDriver()
                {
                    Name = fundDriver.Name,
                    InitialValue = fundDriver.Value,
                    UnlockValue = fundDriver.UnlockValue,
                    CurrentValue = fundDriver.Value,
                    InflationPercent = fundDriver.InflationPercent,
                    BuyCount = 0,
                    Incrementors = incrementors
                });
            }
            return result;
        }

        protected override FundsCounters GetFundsCounters()
        {
            var fundsCounters = _gameData.Counters;
            var counters = new Dictionary<int, CounterBase>();
            var inc = 1;
            foreach (var sourceCounter in fundsCounters.Counters)
            {
                CounterBase destinationCouner = null;
                if (sourceCounter.GetType() == typeof(EntityFX.EconomicsArcade.Contract.Common.Counters.GenericCounter)) {
                     destinationCouner = new GenericCounter();
                    destinationCouner.IsUsedInAutoStep = ((EntityFX.EconomicsArcade.Contract.Common.Counters.GenericCounter)sourceCounter).UseInAutoStep;
                }
                if (destinationCouner != null)
                {
                    destinationCouner.Name = sourceCounter.Name;
                    destinationCouner.SubValue = sourceCounter.Value;
                }
                counters.Add(inc, destinationCouner);
                inc++;
            }

            return new FundsCounters()
            {
                Counters = counters,
                RootCounter = counters[(int)UssrCounterEnum.Communism]
            };
        }

        private object _lockObject = new { };

        protected override void PreInitialize()
        {
            _gameData = _gameDataRepository.GetGameData();
        }

        protected override void PostPerformManualStep()
        {

        }

        protected override void PostPerformAutoStep()
        {



        }

        protected override void PostInitialize()
        {
            //CashFunds(1500000);

        }

        protected override void PostByFundDriver(int fundDriverId)
        {

        }


    }

}
