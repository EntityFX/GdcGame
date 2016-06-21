using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface ICountersRepository
    {
        CounterBase[] FindAll(GetAllCountersCriterion criterion);
    }
}