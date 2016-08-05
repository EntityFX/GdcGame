using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public class FundsDriverRepository : IFundsDriverRepository
    {
        private readonly IMapper<FundsDriverEntity, FundsDriver> _fundsDriverContractMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public FundsDriverRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<FundsDriverEntity, FundsDriver> fundsDriverContractMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _fundsDriverContractMapper = fundsDriverContractMapper;
        }


        public FundsDriver[] FindAll(GetAllFundsDriversCriterion criterion)
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