using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class CounterModelMapper : IMapper<CounterBase, CounterModelBase>
    {
        public CounterModelBase Map(CounterBase source, CounterModelBase destination = null)
        {
            destination = destination ?? new CounterModelBase();
            var singleCounter = source as SingleCounter;
            if (singleCounter != null)
            {
                destination.Name = source.Name;
                destination.Value = source.Value;
                destination.Type = source.Type;
                return destination;
            }

            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                var genericCounterModel = new GenericCounterModel
                {
                    BonusPercentage = genericCounter.BonusPercentage,
                    Bonus = genericCounter.Bonus,
                    Inflation = genericCounter.Inflation,
                    SubValue = genericCounter.SubValue,
                    Name = genericCounter.Name,
                    Value = genericCounter.Value,
                    Type = genericCounter.Type
                };
                return genericCounterModel;
            }

            var delayedCounter = source as DelayedCounter;
            if (delayedCounter != null)
            {
                var genericCounterModel = new DelayedCounterModel
                {
                    SecondsRemaining = delayedCounter.SecondsRemaining,
                    UnlockValue = delayedCounter.UnlockValue,
                    Name = delayedCounter.Name,
                    Value = delayedCounter.Value,
                    Type = delayedCounter.Type
                };
                return genericCounterModel;
            }
            return null;
        }
    }
}
