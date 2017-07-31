namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer
{
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.CustomRule;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

    internal class CustomRuleRepository : ICustomRuleRepository
    {
        private readonly IMapper<CustomRuleEntity, CustomRule> _customRuleMapper;
        private readonly IMapperFactory _mapperFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public CustomRuleRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapperFactory mapperFactory
            )
        {
            this._unitOfWorkFactory = unitOfWorkFactory;
            this._mapperFactory = mapperFactory;
            this._customRuleMapper = this._mapperFactory.Build<CustomRuleEntity, CustomRule>();
        }

        public CustomRule[] FindAll(GetAllCustomRulesCriterion criterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<CustomRuleEntity>>()
                    .With(criterion)
                    .Select(_ => this._customRuleMapper.Map(_))
                    .ToArray();
            }
        }
    }
}