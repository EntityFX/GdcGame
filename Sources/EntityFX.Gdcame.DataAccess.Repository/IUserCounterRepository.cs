using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserCounterRepository
    {
        CounterBase[] FindByUserId(GetUserCountersByUserIdCriterion criterion);
        void CreateForUser(int userId, CounterBase[] counterBases);
        void UpdateForUser(int userId, CounterBase[] counterBases);

    }
}