using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.Infrastructure.Repository;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IUserGameCounterRepository : IRepository<UserGameCounter, GetUserGameCounterByIdCriterion, GetAllCriterion>
    {

    }
}