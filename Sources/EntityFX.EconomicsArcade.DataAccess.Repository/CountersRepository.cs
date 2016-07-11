using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public class CountersRepository : ICountersRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMapper<CounterEntity, CounterBase> _counterContractMapper;

        public CountersRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<CounterEntity, CounterBase> counterContractMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _counterContractMapper = counterContractMapper;
        }
        
        public CounterBase[] FindAll(GetAllCountersCriterion criterion)
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
    }
}