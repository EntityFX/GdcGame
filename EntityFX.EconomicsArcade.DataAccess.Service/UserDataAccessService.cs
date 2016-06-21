using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class UserDataAccessService : IUserDataAccessService
    {
        private readonly IUserRepository _userRepository;

        public UserDataAccessService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public int Create(User user)
        {
            return _userRepository.Create(user);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }

        public void Delete(int userId)
        {
            _userRepository.Delete(userId);
        }

        public User FindById(int userId)
        {
            return _userRepository.FindById(new GetUserByIdCriterion(userId));
        }

        public User[] FindAll()
        {
            return _userRepository.FindAll(new GetAllUsersCriterion());
        }
    }
}
