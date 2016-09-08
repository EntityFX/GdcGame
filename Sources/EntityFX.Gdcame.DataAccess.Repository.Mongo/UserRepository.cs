using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class UserRepository : IUserRepository
    {
        public int Create(DataAccess.Contract.User.User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public DataAccess.Contract.User.User[] FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            throw new NotImplementedException();
        }

        public DataAccess.Contract.User.User[] FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion)
        {
            throw new NotImplementedException();
        }

        public DataAccess.Contract.User.User FindById(GetUserByIdCriterion findByIdCriterion)
        {
            throw new NotImplementedException();
        }

        public DataAccess.Contract.User.User FindByName(GetUserByNameCriterion findByIdCriterion)
        {
            throw new NotImplementedException();
        }

        public void Update(DataAccess.Contract.User.User user)
        {
            throw new NotImplementedException();
        }
    }
}
