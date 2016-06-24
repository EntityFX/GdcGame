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
        private readonly IMapper<UserCounterEntity, CounterBase> _counterContractMapper;
        private readonly IMapper<CounterBase, UserCounterEntity> _userCounterEntityMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserCounterRepository(IUnitOfWorkFactory unitOfWorkFactory,
            IMapper<UserCounterEntity, CounterBase> counterContractMapper,
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
                return findQuery.For<IEnumerable<UserCounterEntity>>()
                    .With(criterion)
                    .Select(_ => _counterContractMapper.Map(_))
                    .ToArray();
            }
        }

        public void CreateForUser(int userId, CounterBase[] counterBases)
        {

            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var counterBase in counterBases)
                {
                    var userCounter = uow.CreateEntity<UserCounterEntity>();
                    userCounter = _userCounterEntityMapper.Map(counterBase, userCounter);
                    userCounter.UserId = userId;
                    userCounter.CreateDateTime = DateTime.Now;
                }
                uow.Commit();
            }
        }

        public void UpdateForUser(int userId, CounterBase[] counterBases)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var counterBase in counterBases)
                {
                    var userCounter = uow.CreateEntity<UserCounterEntity>();
                    userCounter = _userCounterEntityMapper.Map(counterBase, userCounter);
                    userCounter.UserId = userId;
                    userCounter = uow.AttachEntity(userCounter);
                    uow.ExcludeProperty(userCounter,_ => _.CreateDateTime);
                }
                uow.Commit();
            }
        }
    }
}