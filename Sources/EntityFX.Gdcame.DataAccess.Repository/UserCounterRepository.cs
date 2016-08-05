using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository
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