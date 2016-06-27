using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class UserCounterContractMapper : IMapper<UserCounterEntity, CounterBase>
    {
        public CounterBase Map(UserCounterEntity source, CounterBase destination = null)
        {
            if (destination == null)
            {
                switch (source.Counter.Type)
                {
                    case 0:
                        destination = new SingleCounter();
                        break;
                    case 1:
                    case 2:
                        destination = new GenericCounter();
                        var genericDestionation = (GenericCounter)destination;
                        genericDestionation.UseInAutoSteps = source.Counter.UseInAutostep;
                        genericDestionation.Inflation = source.Inflation;
                        genericDestionation.BonusPercentage = source.BonusPercentage;
                        genericDestionation.CurrentSteps = source.CurrentStepsCount;
                        break;
                    case 3:
                        destination = new DelayedCounter();
                        break;
                }
            }
            if (destination == null) return null;
            destination.Name = source.Counter.Name;
            destination.Value = source.Value;
            destination.Type = source.Counter.Type;
            destination.Id = source.Counter.Id;
            return destination;
        }
    }
}