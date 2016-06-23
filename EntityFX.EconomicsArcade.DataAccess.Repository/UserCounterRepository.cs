using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public class UserCounterRepository : IUserCounterRepository
    {
        private readonly IMapper<CounterEntity, CounterBase> _counterContractMapper;
        private readonly IMapper<CounterBase, UserCounterEntity> _userCounterEntityMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserCounterRepository(IUnitOfWorkFactory unitOfWorkFactory,
            IMapper<CounterEntity, CounterBase> counterContractMapper,
            IMapper<CounterBase, UserCounterEntity> userCounterEntityMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _counterContractMapper = counterContractMapper;
            _userCounterEntityMapper = userCounterEntityMapper;
        }

        public CounterBase[] FindByUserId(GetUserCountersByUserIdCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<CounterEntity>>()
                    .With(criterion)
                    .Select(_ => _counterContractMapper.Map(_))
                    .ToArray();
            }
        }

        public void CreateForUser(int userId, CounterBase[] counterBases)
        {
            foreach (var counterBase in counterBases)
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var userCounter = uow.CreateEntity<UserCounterEntity>();
                    userCounter = _userCounterEntityMapper.Map(counterBase, userCounter);
                    userCounter.UserId = userId;
                    uow.Commit();
                }
            }
        }

        public void UpdateForUser(int userId, CounterBase[] counterBases)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var counterBase in counterBases)
                {
                    var userCounter = uow.AttachEntity(uow.CreateEntity<UserCounterEntity>());
                    userCounter = _userCounterEntityMapper.Map(counterBase, userCounter);
                    userCounter.UserId = userId;
                    uow.Commit();
                }
                uow.Commit();
            }
        }
    }
}