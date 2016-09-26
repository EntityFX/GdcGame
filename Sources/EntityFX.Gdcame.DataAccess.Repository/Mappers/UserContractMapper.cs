using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Mappers
{
    public class UserContractMapper : IMapper<UserEntity, User>
    {
        public User Map(UserEntity source, User destination = null)
        {
            destination = destination ?? new User();
            destination.Id = source.Id;
            destination.Login = source.Email;
            destination.IsAdmin = source.IsAdmin;
            destination.PasswordHash = source.Secret;
            return destination;
        }
    }
}