﻿using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef
{
    public class CountersRepository : ICountersRepository
    {
        private readonly IMapper<CounterEntity, CounterBase> _counterContractMapper;
        private readonly IMapperFactory _mapperFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public CountersRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapperFactory mapperFactory
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapperFactory = mapperFactory;
            _counterContractMapper = _mapperFactory.Build<CounterEntity, CounterBase>();
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