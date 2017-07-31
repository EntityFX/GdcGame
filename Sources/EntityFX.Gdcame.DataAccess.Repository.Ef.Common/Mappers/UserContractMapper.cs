namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Mappers
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class UserContractMapper : IMapper<UserEntity, User>
    {
        public User Map(UserEntity source, User destination = null)
        {
            destination = destination ?? new User();
            destination.Id = source.Id;
            destination.Login = source.Email;
            destination.Role = source.Role;
            destination.PasswordHash = source.Secret;
            return destination;
        }
    }
}