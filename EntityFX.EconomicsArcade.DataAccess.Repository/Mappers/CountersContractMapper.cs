using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class CountersContractMapper : IMapper<CounterEntity, CounterBase>
    {
        public CounterBase Map(CounterEntity source, CounterBase destionation = null)
        {
            if (destionation == null)
            {
                switch (source.Type)
                {
                    case 0:
                        destionation = new SingleCounter();
                        break;
                    case 1:
                    case 2:
                        destionation = new GenericCounter();
                        var genericDestionation = (GenericCounter) destionation;
                        genericDestionation.UseInAutoSteps = source.UseInAutoStep;
                        genericDestionation.Inflation = 0;
                        genericDestionation.Bonus = 0;
                        genericDestionation.BonusPercentage = 0;
                        break;
                    case 3:
                        destionation = new DelayedCounter();
                        break;
                }
            }
            if (destionation == null) return null;
            destionation.Name = source.Name;
            destionation.Value = source.InitialValue;
            destionation.Type = source.Type;
            return destionation;
        }
    }
}