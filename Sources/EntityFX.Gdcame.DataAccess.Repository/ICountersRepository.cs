using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.Counters;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface ICountersRepository
    {
        CounterBase[] FindAll(GetAllCountersCriterion criterion);
    }
}