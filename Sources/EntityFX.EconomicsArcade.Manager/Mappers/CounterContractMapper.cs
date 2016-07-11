using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class CounterContractMapper : IMapper<CounterBase, Contract.Common.Counters.CounterBase>
    {
        public Contract.Common.Counters.CounterBase Map(CounterBase source, Contract.Common.Counters.CounterBase destination)
        {
            Contract.Common.Counters.CounterBase destinationCounter = null;
            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                var genericDestination = new Contract.Common.Counters.GenericCounter
                {
                    Bonus = genericCounter.Bonus,
                    BonusPercentage = genericCounter.BonusPercentage,
                    CurrentSteps = genericCounter.CurrentSteps,
                    SubValue = genericCounter.SubValue,
                    Inflation = genericCounter.Inflation,
                    InflationIncreaseSteps = genericCounter.StepsToIncreaseInflation,
                    Type = 1
                };
                destinationCounter = genericDestination;
            }

            var singleCounter = source as SingleCounter;
            if (singleCounter != null)
            {
                destinationCounter = new Contract.Common.Counters.SingleCounter { Value = singleCounter.Value, Type = 0 };
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            var delayedCounter = source as DelayedCounter;
            if (delayedCounter != null)
            {
                destinationCounter = new Contract.Common.Counters.DelayedCounter()
                {
                    MiningTimeSeconds = delayedCounter.SecondsToAchieve, 
                    SecondsRemaining = delayedCounter.SecondsRemaining,
                    UnlockValue = delayedCounter.UnlockValue,
                    Type = 2
                };
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            return  destinationCounter;
        }

        private void MapCommon(CounterBase source, Contract.Common.Counters.CounterBase destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Value = source.Value;
        }
    }
}