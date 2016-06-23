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
        private readonly IMapper<UserCounterEntity, CounterBase> _userCounterContractMapper;
        // private readonly IMapper<FundsDriver, FundsDriverEntity> _fundsDriverEntityMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserCounterRepository(IUnitOfWorkFactory unitOfWorkFactory,
            IMapper<UserCounterEntity, CounterBase> userCounterContractMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userCounterContractMapper = userCounterContractMapper;
            // _fundsDriverEntityMapper = fundsDriverEntityMapper;
        }

        public CounterBase[] FindByUserId(GetUserCountersByUserIdCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserCounterEntity>>()
                    .With(criterion)
                    .Select(_ => _userCounterContractMapper.Map(_))
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
                    var genericCounter = (GenericCounter)counterBase;
                    userCounter.UserId = userId;
                    userCounter.Bonus = genericCounter.Bonus;
                    userCounter.BonusPercentage = genericCounter.BonusPercentage;
                    userCounter.CounterId = genericCounter.Id;
                    userCounter.CreateDateTime = DateTime.Now;
                    //userCounter.DelayedValue = genericCounter.SubValue;
                    userCounter.Value = genericCounter.Value;
                    userCounter.Inflation = genericCounter.Inflation;
                    //userCounter.MiningTimeSecondsEllapsed=genericCounter
                    uow.Commit();
                }
            }
        }

        public void UpdateForUser(int userId, CounterBase[] counterBases)
        {
            foreach (var counterBase in counterBases)
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var userCounter = uow.AttachEntity(uow.CreateEntity<UserCounterEntity>());
                    var genericCounter = (GenericCounter)counterBase;
                    userCounter.UserId = userId;
                    userCounter.Bonus = genericCounter.Bonus;
                    userCounter.BonusPercentage = genericCounter.BonusPercentage;
                    userCounter.CounterId = genericCounter.Id;
                    userCounter.CreateDateTime = DateTime.Now;
                    //userCounter.DelayedValue = genericCounter.SubValue;
                    userCounter.Value = genericCounter.Value;
                    userCounter.Inflation = genericCounter.Inflation;
                    //userCounter.MiningTimeSecondsEllapsed=genericCounter
                    uow.Commit();
                }
            }
        }
    }
}