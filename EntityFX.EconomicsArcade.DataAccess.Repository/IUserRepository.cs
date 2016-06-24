using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.Infrastructure.Repository;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IUserRepository : IRepository<User, GetUserByIdCriterion, GetAllUsersCriterion>
    {
        User FindByName(GetUserByNameCriterion findByIdCriterion);
    }
}