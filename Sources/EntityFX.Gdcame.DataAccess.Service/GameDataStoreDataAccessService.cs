using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Repository;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCustomRuleInfo;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class GameDataStoreDataAccessService : IGameDataStoreDataAccessService
    {
        private readonly IUserGameCounterRepository _userGameCounterRepository;
        private readonly IMapper<GameData, UserGameCounter> _userGameCounterMapper;
        private readonly IUserCounterRepository _userCounterRepository;
        private readonly IUserFundsDriverRepository _userFundsDriverRepository;
        private readonly IUserCustomRuleRepository _userCustomRuleRepository;

        public GameDataStoreDataAccessService(IUserGameCounterRepository userGameCounterRepository
            , IMapper<GameData, UserGameCounter> userGameCounterMapper
            , IUserCounterRepository userCounterRepository
            , IUserFundsDriverRepository userFundsDriverRepository
            , IUserCustomRuleRepository userCustomRuleRepository )
        {
            _userGameCounterRepository = userGameCounterRepository;
            _userGameCounterMapper = userGameCounterMapper;
            _userCounterRepository = userCounterRepository;
            _userFundsDriverRepository = userFundsDriverRepository;
            _userCustomRuleRepository = userCustomRuleRepository;
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

            var userCustomRules = _userCustomRuleRepository.FindByUserId(new GetUserCustomRuleInfoByUserIdCriterion(userId));

            if (userCustomRules == null || userCustomRules.Length == 0)
            {
                _userCustomRuleRepository.CreateForUser(userId, gameData.FundsDrivers.Where(_ => _.CustomRuleInfo != null).Select(_ => _.CustomRuleInfo).ToArray());
            }
            else
            {
                var userCustomRulesToUpdate = gameData.FundsDrivers.Where(_ => _.CustomRuleInfo != null).Select(_ => _.CustomRuleInfo).Where(s => userCustomRules.Any(d => s.CustomRuleId == d.CustomRuleId && s.FundsDriverId == d.FundsDriverId)).ToArray();
                _userCustomRuleRepository.UpdateForUser(userId, userCustomRulesToUpdate);
                var userCustomRulesToCreate = gameData.FundsDrivers.Where(_ => _.CustomRuleInfo != null).Select(_ => _.CustomRuleInfo).Except(userCustomRulesToUpdate).ToArray();
                _userCustomRuleRepository.CreateForUser(userId, userCustomRulesToCreate);
            }
        }
    }

    public class GameDataStoreDataAccessDocumentService : IGameDataStoreDataAccessService
    {
        private readonly IUserGameSnapshotRepository _userGameSnapshotRepository;

        public GameDataStoreDataAccessDocumentService(IUserGameSnapshotRepository userGameSnapshotRepository)
        {
            _userGameSnapshotRepository = userGameSnapshotRepository;
        }

        public void StoreGameDataForUser(int userId, GameData gameData)
        {
            var userGame = _userGameSnapshotRepository.FindByUserId(new GetUserGameSnapshotByIdCriterion(userId));

            if (userGame == null)
            {
                _userGameSnapshotRepository.CreateForUser(userId, gameData);
            }
            else
            {
                _userGameSnapshotRepository.UpdateForUser(userId, userGame);
            }
        }
    } 
}