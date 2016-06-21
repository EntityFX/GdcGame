using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.User
{
    public class GetAllUsersQuery : QueryBase, IQuery<GetAllUsersCriterion, IEnumerable<UserEntity>>
    {
        public GetAllUsersQuery(EconomicsArcadeDbContext dbContext)
            :base(dbContext)
        {

        }

        public IEnumerable<UserEntity> Execute(GetAllUsersCriterion criterion)
        {
            return DbContext.Set<UserEntity>().ToArray();
        }
    }
}
