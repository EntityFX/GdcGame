using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.User;
using EntityFX.Gdcame.Infrastructure.Repository;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserRepository : IRepository<User, GetUserByIdCriterion, GetAllUsersCriterion>
    {
        User FindByName(GetUserByNameCriterion findByIdCriterion);
    }
}