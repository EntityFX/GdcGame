using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class CounterModelMapper : IMapper<CounterBase, CounterModelBase>
    {
        public CounterModelBase Map(CounterBase source, CounterModelBase destination = null)
        {
            destination = destination ?? new CounterModelBase();
            switch (source.Type)
            {
                case 0:
                    var singleCounter = (SingleCounter)source;
                    break;
                case 1:
                case 2:
                    var genericCounter = (GenericCounter)source;
                    var genericeCounterModel = new GenericCounterModel
                    {
                        BonusPercentage = genericCounter.BonusPercentage,
                        Bonus = genericCounter.Bonus,
                        Inflation = genericCounter.Inflation,
                        SubValue = genericCounter.SubValue,
                        UseInAutoSteps = genericCounter.UseInAutoSteps,
                        Value = genericCounter.Value
                    };
                    return destination;
                    break;
                case 3:
                    var delayedCounter = (DelayedCounter)source;
                    break;
            }
            destination.Value = source.Value;
            return destination;
        }
    }
}
