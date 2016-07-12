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
        private readonly IMapper<UserFundsDriverEntity, FundsDriver> _userFundsDriverContractMapper;
        private readonly IMapper<FundsDriver, UserFundsDriverEntity> _userFundsDriverEntityMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserFundsDriverRepository(IUnitOfWorkFactory unitOfWorkFactory,
            IMapper<UserFundsDriverEntity, FundsDriver> userFundsDriverContractMapper,
            IMapper<FundsDriver, UserFundsDriverEntity> userFundsDriverEntityMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userFundsDriverContractMapper = userFundsDriverContractMapper;
            _userFundsDriverEntityMapper = userFundsDriverEntityMapper;
        }

        public FundsDriver[] FindByUserId(GetUserFundsDriverByUserIdCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                return findQuery.For<IEnumerable<UserFundsDriverEntity>>()
                    .With(criterion)
                    .Select(_ => _userFundsDriverContractMapper.Map(_))
                    .ToArray();
            }
        }

        public void CreateForUser(int userId, FundsDriver[] fundsDrivers)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var fundsDriver in fundsDrivers)
                {
                    var userFundsDriver = uow.CreateEntity<UserFundsDriverEntity>();
                    userFundsDriver = _userFundsDriverEntityMapper.Map(fundsDriver, userFundsDriver);
                    userFundsDriver.UserId = userId;
                    //fundsDriver.CreateDateTime = DateTime.Now;
                }
                uow.Commit();
            }
        }

        public void UpdateForUser(int userId, FundsDriver[] fundsDrivers)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var fundsDriver in fundsDrivers)
                {
                    var userFundsDriver = uow.CreateEntity<UserFundsDriverEntity>();
                    userFundsDriver = _userFundsDriverEntityMapper.Map(fundsDriver, userFundsDriver);
                    userFundsDriver.UserId = userId;
                    uow.AttachEntity(userFundsDriver);
                    //fundsDriver.CreateDateTime = DateTime.Now;
                }
                uow.Commit();
            }
        }
    }
}