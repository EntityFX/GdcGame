using EntityFX.EconomicsArcade.Infrastructure.Repository;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserGameCounterRepository : IRepository<UserGameCounter, GetUserGameCounterByIdCriterion, GetAllCriterion>
    {

    }
}