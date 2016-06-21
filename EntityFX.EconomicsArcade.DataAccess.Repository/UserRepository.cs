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
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IMapper<User, UserEntity> _userEntityMapper;

        private readonly IMapper<UserEntity, User> _userContractMapper;
        
        public UserRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<User, UserEntity> userEntityMapper
            , IMapper<UserEntity, User> userContractMapper
        )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userEntityMapper = userEntityMapper;
            _userContractMapper = userContractMapper;
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

        public void Delete(int id)
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

        }

        public User[] FindAll()
        {
            throw new NotImplementedException();
        }


        public User FindById(GetUserByIdCriterion findByIdCriterion)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserEntity>()
                    .With(findByIdCriterion);
                return _userContractMapper.Map(entity);
            }
        }

        public User[] FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserEntity>>()
                    .With(finalAllCriterion)
                    .Select(_ => _userContractMapper.Map(_))
                    .ToArray();
            }
        }
    }
}
