using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreCounterContractMapper : IMapper<CounterBase, StoreCounterBase>
    {
        public StoreCounterBase Map(CounterBase source, StoreCounterBase destination)
        {
            StoreCounterBase destinationCounter = null;
            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                var genericDestination = new StoreGenericCounter
                {
                    BonusPercent = genericCounter.BonusPercentage,
                    CurrentSteps = genericCounter.CurrentSteps,
                    Inflation = genericCounter.Inflation,
                    Type = 1
                };
                destinationCounter = genericDestination;
            }

            var singleCounter = source as SingleCounter;
            if (singleCounter != null)
            {
                destinationCounter = new StoreSingleCounter { Value = singleCounter.Value, Type = 0 };
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            var delayedCounter = source as DelayedCounter;
            if (delayedCounter != null)
            {
                destinationCounter = new StoreDelayedCounter()
                {
                    SecondsRemaining = delayedCounter.SecondsRemaining,
                    DelayedValue = delayedCounter.SubValue,
                    Type = 2
                };
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            return destinationCounter;
        }

        private void MapCommon(CounterBase source, StoreCounterBase destination)
        {
            destination.Id = source.Id;
            destination.Value = source.Value;
        }
    }
}