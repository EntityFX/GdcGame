using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;

namespace EntityFX.Gdcame.DataAccess.Service
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

        public void Delete(string userId)
        {
            _userRepository.Delete(userId);
        }

        public User FindById(string userId)
        {
            return _userRepository.FindById(new GetUserByIdCriterion(userId));
        }

        public User FindByName(string name)
        {
            return _userRepository.FindByName(new GetUserByNameCriterion(name));
        }

        public User[] FindByFilter(string searchString)
        {
            return _userRepository.FindByFilter(new GetUsersBySearchStringCriterion(searchString));
        }

        public User[] FindAll()
        {
            return _userRepository.FindAll(new GetAllUsersCriterion());
        }
    }
}