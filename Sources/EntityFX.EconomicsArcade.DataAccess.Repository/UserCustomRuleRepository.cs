using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCustomRuleInfo;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    class UserCustomRuleRepository : IUserCustomRuleRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMapper<UserCustomRuleEntity, CustomRuleInfo> _userCustomRuleContractMapper;
        private readonly IMapper<CustomRuleInfo, UserCustomRuleEntity> _userCustomRuleEntityMapper;

        public UserCustomRuleRepository(IUnitOfWorkFactory unitOfWorkFactory,
            IMapper<UserCustomRuleEntity, CustomRuleInfo> userCustomRuleContractMapper,
            IMapper<CustomRuleInfo, UserCustomRuleEntity> userCustomRuleEntityMapper)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userCustomRuleContractMapper = userCustomRuleContractMapper;
            _userCustomRuleEntityMapper = userCustomRuleEntityMapper;
        }

        public CustomRuleInfo[] FindByUserId(GetUserCustomRuleInfoByUserIdCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserCustomRuleEntity>>()
                    .With(criterion)
                    .Select(_ => _userCustomRuleContractMapper.Map(_))
                    .ToArray();
            }
        }

        public void CreateForUser(int userId, CustomRuleInfo[] fundsDrivers)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var fundsDriver in fundsDrivers)
                {
                    var userFundsDriver = uow.CreateEntity<UserCustomRuleEntity>();
                    userFundsDriver = _userCustomRuleEntityMapper.Map(fundsDriver, userFundsDriver);
                    userFundsDriver.UserId = userId;
                    //fundsDriver.CreateDateTime = DateTime.Now;
                }
                uow.Commit();
            }
        }

        public void UpdateForUser(int userId, CustomRuleInfo[] fundsDrivers)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var fundsDriver in fundsDrivers)
                {
                    var userFundsDriver = uow.CreateEntity<UserCustomRuleEntity>();
                    userFundsDriver = _userCustomRuleEntityMapper.Map(fundsDriver, userFundsDriver);
                    userFundsDriver.UserId = userId;
                    uow.AttachEntity(userFundsDriver);
                    //fundsDriver.CreateDateTime = DateTime.Now;
                }
                uow.Commit();
            }
        }
    }
}