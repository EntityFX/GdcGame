using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Service.Mappers
{
    public class UserEntityMapper : IMapper<User, UserEntity>
    {
        public UserEntity Map(User source, UserEntity destination)
        {
            destination.Id = source.Id;
                destination.Email = source.Email;
                return destination;
        }
    }
}
