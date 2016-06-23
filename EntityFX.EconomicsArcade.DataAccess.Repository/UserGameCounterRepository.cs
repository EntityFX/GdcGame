using System;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
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
                var userGameCounterEntity = uow.AttachEntity(uow.CreateEntity<UserGameCounterEntity>());
                userGameCounterEntity = _userGameCounterEntityMapper.Map(userGameCounter, userGameCounterEntity);
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