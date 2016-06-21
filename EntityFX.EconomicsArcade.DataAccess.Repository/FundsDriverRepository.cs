using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public class FundsDriverRepository : IFundsDriverRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        
        private readonly IMapper<FundsDriverEntity, FundsDriver> _fundsDriverContractMapper;

        public FundsDriverRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<FundsDriverEntity, FundsDriver> fundsDriverContractMapper
        )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _fundsDriverContractMapper = fundsDriverContractMapper;
        }
        
        
        public FundsDriver[] FindAll(Criterions.FundsDriver.GetAllFundsDriversCriterion criterion)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
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
