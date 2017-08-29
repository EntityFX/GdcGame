using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Contract.MainServer.GameDataTransfer;
using EntityFX.Gdcame.DataAccess.Contract.Common.User;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    public class DataTransferContractMapper : IMapper<UserDataTransfer,User>
    {
        public User Map(UserDataTransfer source, User destination = null)
        {

            var user = new User()
            {
                Id = source.Id,
                Login = source.Login,
                PasswordHash = source.PasswordHash,
                Role = source.Role
            };
            return user;
        }
    }
}
