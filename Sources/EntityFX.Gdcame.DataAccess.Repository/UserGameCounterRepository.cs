using System;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.Criterion;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public class UserGameCounterRepository : IUserGameCounterRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IMapper<UserGameCounterEntity, UserGameCounter> _userGameCounterContractMapper;

        private readonly IMapper<UserGameCounter, UserGameCounterEntity> _userGameCounterEntityMapper;

        public UserGameCounterRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<UserGameCounterEntity, UserGameCounter> userGameCounterContractMapper
            , IMapper<UserGameCounter, UserGameCounterEntity> userGameCounterEntityMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userGameCounterContractMapper = userGameCounterContractMapper;
            _userGameCounterEntityMapper = userGameCounterEntityMapper;
        }

        public int Create(UserGameCounter userGameCounter)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userGameCounterEntity = uow.CreateEntity<UserGameCounterEntity>();
                userGameCounterEntity = _userGameCounterEntityMapper.Map(userGameCounter, userGameCounterEntity);
                userGameCounterEntity.CreateDateTime = DateTime.Now;
                uow.Commit();
            }
            return 0;
        }

        public void Update(UserGameCounter userGameCounter)
        {

            using (var uow = _unitOfWorkFactory.Create())
            {
                var userGameCounterEntity = uow.CreateEntity<UserGameCounterEntity>();
                userGameCounterEntity = _userGameCounterEntityMapper.Map(userGameCounter, userGameCounterEntity);
                uow.AttachEntity(userGameCounterEntity);
                uow.ExcludeProperty(userGameCounterEntity, entity => entity.CreateDateTime);
                userGameCounterEntity.CreateDateTime = DateTime.Now;
                uow.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userGameCounterEntity =
                    uow.BuildQuery().For<UserGameCounterEntity>().With(new GetUserGameCounterByIdCriterion(id));
                uow.DeleteEntity(userGameCounterEntity);
                uow.Commit();
            }
        }

        public UserGameCounter FindById(GetUserGameCounterByIdCriterion findByIdCriterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserGameCounterEntity>()
                    .With(findByIdCriterion);
                return entity != null ? _userGameCounterContractMapper.Map(entity) : null;
            }
        }

        [Obsolete]
        public UserGameCounter[] FindAll(GetAllCriterion finalAllCriterion)
        {
            throw new OperationCanceledException();
        }
    }
}