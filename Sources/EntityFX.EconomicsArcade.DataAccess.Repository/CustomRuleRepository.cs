using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.CustomRule;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    class CustomRuleRepository : ICustomRuleRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMapper<CustomRuleEntity, CustomRule> _customRuleMapper;

        public CustomRuleRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<CustomRuleEntity, CustomRule> customRuleMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _customRuleMapper = customRuleMapper;
        }

        public CustomRule[] FindAll(GetAllCustomRulesCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<CustomRuleEntity>>()
                    .With(criterion)
                    .Select(_ => _customRuleMapper.Map(_))
                    .ToArray();
            }
        }
    }
}