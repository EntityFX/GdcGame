namespace EntityFX.Gdcame.DataAccess.Service.Common
{
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;

    public class UserDataAccessService : IUserDataAccessService
    {
        private readonly IUserRepository _userRepository;

        public UserDataAccessService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public int Create(User user)
        {
            return this._userRepository.Create(user);
        }

        public void Update(User user)
        {
            this._userRepository.Update(user);
        }

        public void Delete(string userId)
        {
            this._userRepository.Delete(userId);
        }

        public User FindById(string userId)
        {
            return this._userRepository.FindById(new GetUserByIdCriterion(userId));
        }

        public User FindByName(string name)
        {
            return this._userRepository.FindByName(new GetUserByNameCriterion(name));
        }

        public User[] FindByFilter(string searchString)
        {
            return this._userRepository.FindByFilter(new GetUsersBySearchStringCriterion(searchString)).ToArray();
        }

        public User[] FindAll()
        {
            return this._userRepository.FindAll(new GetAllUsersCriterion()).ToArray();
        }

        public int Count()
        {
            return this._userRepository.Count();
        }

        public User[] FindChunked(int offset, int count)
        {
            return this._userRepository.FindChunked(new GetUsersByOffsetCriterion(offset, count)).ToArray();
        }
    }
}