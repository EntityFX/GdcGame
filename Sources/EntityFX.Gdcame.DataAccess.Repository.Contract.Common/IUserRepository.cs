namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common
{
    using System.Collections.Generic;

    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.Infrastructure.Repository;

    public interface IUserRepository : IRepository<User, GetUserByIdCriterion, GetAllUsersCriterion>
    {
        User FindByName(GetUserByNameCriterion findByIdCriterion);

        IEnumerable<User> FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion);

        IEnumerable<User> FindChunked(GetUsersByOffsetCriterion offsetCriterion);

        int Count();
    }
}