using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.Infrastructure.Repository;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IUserCounterRepository : IRepository<CounterBase, GetUserCountersByUserIdCriterion, GetAllUsersCriterion>
    {

    }
}