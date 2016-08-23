using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.CustomRule;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef
{
    internal class CustomRuleRepository : ICustomRuleRepository
    {
        private readonly IMapper<CustomRuleEntity, CustomRule> _customRuleMapper;
        private readonly IMapperFactory _mapperFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public CustomRuleRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapperFactory mapperFactory
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapperFactory = mapperFactory;
            _customRuleMapper = _mapperFactory.Build<CustomRuleEntity, CustomRule>();
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