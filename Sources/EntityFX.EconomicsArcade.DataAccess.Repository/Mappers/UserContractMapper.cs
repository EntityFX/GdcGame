using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class UserContractMapper : IMapper<UserEntity, User>
    {
        public User Map(UserEntity source, User destination = null)
        {
            destination = destination ?? new User();
            destination.Id = source.Id;
            destination.Email = source.Email;
            destination.IsAdmin = source.IsAdmin;
            destination.PasswordHash = source.Secret;
            return destination;
        }
    }
}