using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class UserContractMapper : IMapper<UserEntity, User>
    {
        public User Map(UserEntity source, User destination = null)
        {
            destination = new User();
            destination.Id = source.Id;
            destination.Email = source.Email;
            return destination;
        }
    }
}
