using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
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