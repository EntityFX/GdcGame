using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface ICountersRepository
    {
        CounterBase[] FindAll(GetAllCountersCriterion criterion);
    }
}