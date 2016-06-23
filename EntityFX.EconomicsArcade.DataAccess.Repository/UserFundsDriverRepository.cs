using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public class UserFundsDriverRepository : IUserFundsDriverRepository
    {
        private readonly IMapper<FundsDriverEntity, FundsDriver> _fundsDriverContractMapper;
        private readonly IMapper<FundsDriver, FundsDriverEntity> _fundsDriverEntityMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserFundsDriverRepository(IUnitOfWorkFactory unitOfWorkFactory,
            IMapper<FundsDriverEntity, FundsDriver> fundsDriverContractMapper,
            IMapper<FundsDriver, FundsDriverEntity> fundsDriverEntityMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _fundsDriverContractMapper = fundsDriverContractMapper;
            _fundsDriverEntityMapper = fundsDriverEntityMapper;
        }

        public FundsDriver[] FindByUserId(GetFundsDriverByUserIdCriterion criterion)
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

        public void CreateForUser(int userId, FundsDriver fundsDriver)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userFundsDriver = uow.CreateEntity<UserFundsDriverEntity>();
                userFundsDriver.UserId = userId;
                userFundsDriver.BuyCount = fundsDriver.BuyCount;
                userFundsDriver.CreateDateTime = DateTime.Now;
                userFundsDriver.FundsDriverId = fundsDriver.Id;
                userFundsDriver.Value = fundsDriver.Value;
                uow.Commit();
            }
        }

        public void UpdateForUser(int userId, FundsDriver fundsDriver)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userFundsDriver = uow.AttachEntity(uow.CreateEntity<UserFundsDriverEntity>());
                userFundsDriver.UserId = userId;
                userFundsDriver.BuyCount = fundsDriver.BuyCount;
                userFundsDriver.CreateDateTime = DateTime.Now;
                userFundsDriver.FundsDriverId = fundsDriver.Id;
                userFundsDriver.Value = fundsDriver.Value;
                uow.Commit();
            }
        }
    }
}