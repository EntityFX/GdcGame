namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer
{
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Common.Contract.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

    public class CountersRepository : ICountersRepository
    {
        private readonly IMapper<CounterEntity, CounterBase> _counterContractMapper;
        private readonly IMapperFactory _mapperFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public CountersRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapperFactory mapperFactory
            )
        {
            this._unitOfWorkFactory = unitOfWorkFactory;
            this._mapperFactory = mapperFactory;
            this._counterContractMapper = this._mapperFactory.Build<CounterEntity, CounterBase>();
        }

        public CounterBase[] FindAll(GetAllCountersCriterion criterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<CounterEntity>>()
                    .With(criterion)
                    .Select(_ => this._counterContractMapper.Map(_))
                    .ToArray();
            }
        }
    }
}