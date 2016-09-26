using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;
using EntityFX.Gdcame.DataAccess.Contract.User;

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
            return new User() { Id = "1", Login = "admin", IsAdmin = true, PasswordHash = @"AEaKRfyF9sOQcAm3L+PrN67RZVWrutGFPLBMo9Tau+4uKcDTDQgqiz6v8spTtLAZcg==" };
        }

        public void Update(DataAccess.Contract.User.User user)
        {
            throw new NotImplementedException();
        }
    }
}
