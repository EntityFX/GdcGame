using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IMapper<UserEntity, User> _userContractMapper;
        private readonly IMapper<User, UserEntity> _userEntityMapper;

        public UserRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapperFactory mapperFactory
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapperFactory = mapperFactory;
            _userEntityMapper = _mapperFactory.Build<User, UserEntity>();
            _userContractMapper = _mapperFactory.Build<UserEntity, User>();
        }

        public int Create(User user)
        {
            using (var uow = _unitOfWorkFactory.Create())
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

        public void Delete(string id)
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


        public User FindById(GetUserByIdCriterion findByIdCriterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserEntity>()
                    .With(findByIdCriterion);
                return entity != null ? _userContractMapper.Map(entity) : null;
            }
        }

        public User[] FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserEntity>>()
                    .With(finalAllCriterion)
                    .Select(_ => _userContractMapper.Map(_))
                    .ToArray();
            }
        }

        public User FindByName(GetUserByNameCriterion findByIdCriterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserEntity>()
                    .With(findByIdCriterion);
                return entity != null ? _userContractMapper.Map(entity) : null;
            }
        }

        public User[] FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserEntity>>()
                    .With(findByIdCriterion)
                    .Select(_ => _userContractMapper.Map(_))
                    .ToArray();
            }
        }
    }
}