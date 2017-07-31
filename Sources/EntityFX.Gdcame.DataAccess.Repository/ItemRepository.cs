namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer
{
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Common.Contract.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

    public class ItemRepository : IItemRepository
    {
        private readonly IMapper<FundsDriverEntity, Item> _fundsDriverContractMapper;
        private readonly IMapperFactory _mapperFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ItemRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapperFactory mapperFactory
            )
        {
            this._unitOfWorkFactory = unitOfWorkFactory;
            this._mapperFactory = mapperFactory;
            this._fundsDriverContractMapper = this._mapperFactory.Build<FundsDriverEntity, Item>();
        }


        public Item[] FindAll(GetAllFundsDriversCriterion criterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<FundsDriverEntity>>()
                    .With(criterion)
                    .Select(_ => this._fundsDriverContractMapper.Map(_))
                    .ToArray();
            }
        }
    }
}