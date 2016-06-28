using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class CountersContractMapper : IMapper<CounterEntity, CounterBase>
    {
        public CounterBase Map(CounterEntity source, CounterBase destination = null)
        {
           destination = destination ?? new UserCounterEntity();
            switch (source.Type)
            {
                case 0:
                    var singleCounter = (SingleCounter)source;
                    break;
                case 1:
                case 2:
                    var genericCounter = (GenericCounter)source;
                    destination.BonusPercentage = genericCounter.BonusPercentage;
                    destination.CounterId = genericCounter.Id;
                    destination.CreateDateTime = DateTime.Now;
                    destination.Inflation = genericCounter.Inflation;
                    destination.CurrentStepsCount = genericCounter.CurrentSteps;
                    break;
                case 3:
                    var delayedCounter = (DelayedCounter)source;
                    break;
            }
            destination.CounterId = source.Id;
            destination.Value = source.Value;
            return destination;
        }
        }
    }
}