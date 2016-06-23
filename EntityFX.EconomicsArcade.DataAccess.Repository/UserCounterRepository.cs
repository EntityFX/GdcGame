using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public class UserCounterRepository : IUserCounterRepository
    {
         private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IMapper<UserGameCounterEntity, UserGameCounter> _userGameCounterContractMapper;

        private readonly IMapper<UserGameCounter, UserGameCounterEntity> _userGameCounterEntityMapper;

        public UserGameCounterRepository(IUnitOfWorkFactory unitOfWorkFactory
            , IMapper<UserGameCounterEntity, UserGameCounter> userGameCounterContractMapper
            , IMapper<UserGameCounter, UserGameCounterEntity> userGameCounterEntityMapper
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userGameCounterContractMapper = userGameCounterContractMapper;
            _userGameCounterEntityMapper = userGameCounterEntityMapper;
        }

        public int Create(CounterBase user)
        {
            throw new System.NotImplementedException();
        }

        public void Update(CounterBase user)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public CounterBase FindById(GetUserCountersByUserIdCriterion findByIdCriterion)
        {
            throw new System.NotImplementedException();
        }

        public CounterBase[] FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            throw new System.NotImplementedException();
        }
    }
}