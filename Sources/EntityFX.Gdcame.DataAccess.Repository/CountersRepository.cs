using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository
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