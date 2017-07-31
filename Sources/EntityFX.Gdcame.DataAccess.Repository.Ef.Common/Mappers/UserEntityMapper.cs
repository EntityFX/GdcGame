namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Mappers
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class UserEntityMapper : IMapper<User, UserEntity>
    {
        public UserEntity Map(User source, UserEntity destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Login;
            destination.Role= source.Role;
            destination.Secret = source.PasswordHash;
            return destination;
        }
    }
}