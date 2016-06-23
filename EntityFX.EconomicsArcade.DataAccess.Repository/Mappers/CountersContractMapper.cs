using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class CountersContractMapper : IMapper<CounterEntity, CounterBase>
    {
        public CounterBase Map(CounterEntity source, CounterBase destination = null)
        {
            if (destination == null)
            {
                switch (source.Type)
                {
                    case 0:
                        destination = new SingleCounter();
                        break;
                    case 1:
                    case 2:
                        destination = new GenericCounter();
                        var genericDestionation = (GenericCounter) destination;
                        genericDestionation.UseInAutoSteps = source.UseInAutoStep;
                        genericDestionation.Inflation = 0;
                        genericDestionation.Bonus = 0;
                        genericDestionation.BonusPercentage = 0;
                        break;
                    case 3:
                        destination = new DelayedCounter();
                        break;
                }
            }
            if (destination == null) return null;
            destination.Name = source.Name;
            destination.Value = source.InitialValue;
            destination.Type = source.Type;
            destination.Id = source.Id;
            return destination;
        }
    }
}