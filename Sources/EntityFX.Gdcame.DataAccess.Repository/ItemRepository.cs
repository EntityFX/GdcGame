﻿using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMapper<FundsDriverEntity, Item> _fundsDriverContractMapper;
        private readonly IMapperFactory _mapperFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ItemRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapperFactory mapperFactory
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapperFactory = mapperFactory;
            _fundsDriverContractMapper = _mapperFactory.Build<FundsDriverEntity, Item>();
        }


        public Item[] FindAll(GetAllFundsDriversCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<FundsDriverEntity>>()
                    .With(criterion)
                    .Select(_ => _fundsDriverContractMapper.Map(_))
                    .ToArray();
            }
        }
    }
}