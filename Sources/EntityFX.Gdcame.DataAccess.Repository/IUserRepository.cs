using EntityFX.EconomicsArcade.Infrastructure.Repository;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.User;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserRepository : IRepository<User, GetUserByIdCriterion, GetAllUsersCriterion>
    {
        User FindByName(GetUserByNameCriterion findByIdCriterion);
    }
}