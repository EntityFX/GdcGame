using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef
{
    public class FundsDriverRepository : IFundsDriverRepository
    {
        private readonly IMapper<FundsDriverEntity, Item> _fundsDriverContractMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public FundsDriverRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<FundsDriverEntity, Item> fundsDriverContractMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _fundsDriverContractMapper = fundsDriverContractMapper;
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