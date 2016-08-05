using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
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