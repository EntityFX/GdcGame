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
    public class GetUserByIdQuery : QueryBase, IQuery<GetUserByIdCriterion, UserEntity>
    {
        public GetUserByIdQuery(EconomicsArcadeDbContext dbContext)
            :base(dbContext)
        {

        }
        
        public UserEntity Execute(GetUserByIdCriterion criterion)
        {
            return DbContext.Set<UserEntity>().SingleOrDefault(_ => _.Id == criterion.Id);
        }
    }
}
