﻿using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Mappers
{
    public class UserEntityMapper : IMapper<User, UserEntity>
    {
        public UserEntity Map(User source, UserEntity destination)
        {
            destination.Id = source.Id.ToString();///////
            destination.Email = source.Email;
            destination.IsAdmin = source.IsAdmin;
            destination.Secret = source.PasswordHash;
            return destination;
        }
    }
}