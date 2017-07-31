using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.Kernel.Contract.Counters;

    public class StoreCounterContractMapper : IMapper<CounterBase, StoredCounterBase>
    {
        public StoredCounterBase Map(CounterBase source, StoredCounterBase destination)
        {
            StoredCounterBase destinationCounter = null;
            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                var genericDestination = new StoredGenericCounter
                {
                    BonusPercent = genericCounter.BonusPercentage,
                    CurrentSteps = genericCounter.CurrentSteps,
                    Inflation = genericCounter.Inflation
                };
                destinationCounter = genericDestination;
            }

            var singleCounter = source as SingleCounter;
            if (singleCounter != null)
            {
                destinationCounter = new StoredSingleCounter {Value = singleCounter.Value};
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            var delayedCounter = source as DelayedCounter;
            if (delayedCounter != null)
            {
                destinationCounter = new StoredDelayedCounter
                {
                    SecondsRemaining = delayedCounter.SecondsRemaining,
                    DelayedValue = delayedCounter.SubValue
                };
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            return destinationCounter;
        }

        private void MapCommon(CounterBase source, StoredCounterBase destination)
        {
            destination.Id = source.Id;
            destination.Value = source.Value;
        }
    }
}