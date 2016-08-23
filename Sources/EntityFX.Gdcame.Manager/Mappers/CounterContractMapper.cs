using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class CounterContractMapper : IMapper<CounterBase, Common.Contract.Counters.CounterBase>
    {
        public Common.Contract.Counters.CounterBase Map(CounterBase source,
            Common.Contract.Counters.CounterBase destination)
        {
            Common.Contract.Counters.CounterBase destinationCounter = null;
            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                var genericDestination = new Common.Contract.Counters.GenericCounter
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
                destinationCounter = new Common.Contract.Counters.SingleCounter {Value = singleCounter.Value, Type = 0};
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            var delayedCounter = source as DelayedCounter;
            if (delayedCounter != null)
            {
                destinationCounter = new Common.Contract.Counters.DelayedCounter
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

            return destinationCounter;
        }

        private void MapCommon(CounterBase source, Common.Contract.Counters.CounterBase destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Value = source.Value;
        }
    }
}