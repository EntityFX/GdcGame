namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

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
            this._unitOfWorkFactory = unitOfWorkFactory;
            this._mapperFactory = mapperFactory;
            this._userEntityMapper = this._mapperFactory.Build<User, UserEntity>();
            this._userContractMapper = this._mapperFactory.Build<UserEntity, User>();
        }

        public int Create(User user)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var userEntity = uow.CreateEntity<UserEntity>();
                userEntity = this._userEntityMapper.Map(user, userEntity);
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
            //    userEntity.Login = user.Login;
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
            //    userEntity.Login = user.Login;
            //    userEntity.CreateDateTime = DateTime.Now;
            //    uow.Commit();
            //}
        }


        public User FindById(GetUserByIdCriterion findByIdCriterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserEntity>()
                    .With(findByIdCriterion);
                return entity != null ? this._userContractMapper.Map(entity) : null;
            }
        }

        public IEnumerable<User> FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserEntity>>()
                    .With(finalAllCriterion)
                    .Select(_ => this._userContractMapper.Map(_))
                    .ToList();
            }
        }

        public User FindByName(GetUserByNameCriterion findByIdCriterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserEntity>()
                    .With(findByIdCriterion);
                return entity != null ? this._userContractMapper.Map(entity) : null;
            }
        }

        public IEnumerable<User> FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserEntity>>()
                    .With(findByIdCriterion)
                    .Select(_ => this._userContractMapper.Map(_))
                    .ToList();
            }
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> FindChunked(GetUsersByOffsetCriterion offsetCriterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserEntity>>()
                    .With(offsetCriterion)
                    .Select(_ => this._userContractMapper.Map(_))
                    .ToList();
            }
        }
    }
}