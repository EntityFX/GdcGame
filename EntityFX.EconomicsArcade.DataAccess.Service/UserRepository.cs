using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
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
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IMapper<User, UserEntity> _userEntityMapper;
        
        public UserRepository(IUnitOfWorkFactory unitOfWorkFactory, IMapper<User, UserEntity> userEntityMapper)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userEntityMapper = userEntityMapper;
        }
        
        public int Create(User user)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var userEntity = uow.CreateEntity<UserEntity>();
                userEntity = _userEntityMapper.Map(user, userEntity);
                userEntity.CreateDateTime = DateTime.Now;
                uow.Commit();
            }
            return 0;
        }

        public void Update(User user)
        {
            //using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            //{
            //    var userEntity = uow.
            //    userEntity.Id = user.Id;
            //    userEntity.Email = user.Email;
            //    userEntity.CreateDateTime = DateTime.Now;
            //    uow.Commit();
            //}
        }

        public void Delete(int userId)
        {
            //using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            //{
            //    var userEntity = uow.DeleteEntity<UserEntity>()
            //    userEntity.Id = user.Id;
            //    userEntity.Email = user.Email;
            //    userEntity.CreateDateTime = DateTime.Now;
            //    uow.Commit();
            //}
        }

        public void FindById(int userId)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<User>()
                    .With(new GetByIdCriterion(userId));

            }
        }

        public User[] FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
