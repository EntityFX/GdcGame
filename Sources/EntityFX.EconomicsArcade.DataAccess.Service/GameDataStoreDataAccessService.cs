using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class GameDataStoreDataAccessService : IGameDataStoreDataAccessService
    {
        private readonly IUserGameCounterRepository _userGameCounterRepository;
        private readonly IMapper<GameData, UserGameCounter> _userGameCounterMapper;
        private readonly IUserCounterRepository _userCounterRepository;
        private readonly IUserFundsDriverRepository _userFundsDriverRepository;

        public GameDataStoreDataAccessService(IUserGameCounterRepository userGameCounterRepository
            , IMapper<GameData, UserGameCounter> userGameCounterMapper
            , IUserCounterRepository userCounterRepository
            , IUserFundsDriverRepository userFundsDriverRepository )
        {
            _userGameCounterRepository = userGameCounterRepository;
            _userGameCounterMapper = userGameCounterMapper;
            _userCounterRepository = userCounterRepository;
            _userFundsDriverRepository = userFundsDriverRepository;
        }

        public void StoreGameDataForUser(int userId, GameData gameData)
        {
           
            var userGameCounter = _userGameCounterRepository.FindById(new GetUserGameCounterByIdCriterion(userId));

            if (userGameCounter == null)
            {
                userGameCounter = _userGameCounterMapper.Map(gameData);
                userGameCounter.UserId = userId;
                _userGameCounterRepository.Create(userGameCounter);
            }
            else
            {
                userGameCounter = _userGameCounterMapper.Map(gameData);
                userGameCounter.UserId = userId;
                _userGameCounterRepository.Update(userGameCounter);
            }

            var userCounters = _userCounterRepository.FindByUserId(new GetUserCountersByUserIdCriterion(userId));
            if (userCounters == null || userCounters.Length == 0)
            {
                _userCounterRepository.CreateForUser(userId, gameData.Counters.Counters);
            }
            else
            {
                var countersToUpdate = gameData.Counters.Counters.Where(s => userCounters.Any(d => s.Id == d.Id)).ToArray();
                _userCounterRepository.UpdateForUser(userId, countersToUpdate);
                var countersToCreate = gameData.Counters.Counters.Except(countersToUpdate).ToArray();
                _userCounterRepository.CreateForUser(userId, countersToCreate);
            }

            var userFundsDriver = _userFundsDriverRepository.FindByUserId(new GetUserFundsDriverByUserIdCriterion(userId));
            if (userFundsDriver == null || userCounters.Length == 0)
            {
                _userFundsDriverRepository.CreateForUser(userId, gameData.FundsDrivers);
            }
            else
            {
                var userFundsDriverToUpdate = gameData.FundsDrivers.Where(s => userFundsDriver.Any(d => s.Id == d.Id)).ToArray();
                _userFundsDriverRepository.UpdateForUser(userId, userFundsDriverToUpdate);
                var userFundsDriverToCreate = gameData.FundsDrivers.Except(userFundsDriverToUpdate).ToArray();
                _userFundsDriverRepository.CreateForUser(userId, userFundsDriverToCreate);
            }
        }
    }
}