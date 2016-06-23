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
        private readonly IMapper<FundsDriver, UserFundsDriverEntity> _userFundsDriverEntityMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserFundsDriverRepository(IUnitOfWorkFactory unitOfWorkFactory,
            IMapper<FundsDriverEntity, FundsDriver> fundsDriverContractMapper,
            IMapper<FundsDriver, UserFundsDriverEntity> userFundsDriverEntityMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _fundsDriverContractMapper = fundsDriverContractMapper;
            _userFundsDriverEntityMapper = userFundsDriverEntityMapper;
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
                userFundsDriver = _userFundsDriverEntityMapper.Map(fundsDriver, userFundsDriver);
                userFundsDriver.UserId = userId;
                uow.Commit();
            }
        }

        public void UpdateForUser(int userId, FundsDriver fundsDriver)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userFundsDriver = uow.AttachEntity(uow.CreateEntity<UserFundsDriverEntity>());
                userFundsDriver = _userFundsDriverEntityMapper.Map(fundsDriver, userFundsDriver);
                userFundsDriver.UserId = userId;
                uow.Commit();
            }
        }
    }
}