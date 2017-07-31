using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    using EntityFX.Gdcame.Kernel.Contract.Counters;

    public class CounterContractMapper : IMapper<CounterBase, Gdcame.Contract.MainServer.Counters.CounterBase>
    {
        public Gdcame.Contract.MainServer.Counters.CounterBase Map(CounterBase source,
            Gdcame.Contract.MainServer.Counters.CounterBase destination)
        {
            Gdcame.Contract.MainServer.Counters.CounterBase destinationCounter = null;
            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                var genericDestination = new Gdcame.Contract.MainServer.Counters.GenericCounter
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
                destinationCounter = new Gdcame.Contract.MainServer.Counters.SingleCounter {Value = singleCounter.Value, Type = 0};
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            var delayedCounter = source as DelayedCounter;
            if (delayedCounter != null)
            {
                destinationCounter = new Gdcame.Contract.MainServer.Counters.DelayedCounter
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

        private void MapCommon(CounterBase source, Gdcame.Contract.MainServer.Counters.CounterBase destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Value = source.Value;
        }
    }
}