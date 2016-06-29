using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using System;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Manager.Mappers;
using FundsCounters = EntityFX.EconomicsArcade.Contract.Common.Counters.FundsCounters;
using EntityFX.EconomicsArcade.Contract.Common;

namespace EntityFX.EconomicsArcade.Manager
{
    public class GameManager : IGameManager
    {
        private readonly GameSessions _gameSessions;
        private readonly GameDataContractMapper _gameDataContractMapper;
        private readonly FundsCountersContractMapper _countersContractMapper;

        public GameManager(GameSessions gameSessions
            , GameDataContractMapper gameDataContractMapper
            , FundsCountersContractMapper countersContractMapper)
        {
            _gameSessions = gameSessions;
            _gameDataContractMapper = gameDataContractMapper;
            _countersContractMapper = countersContractMapper;
        }

        public void BuyFundDriver(int fundDriverId)
        {
            GetSessionGame().BuyFundDriver(fundDriverId);
        }

        public void PerformManualStep()
        {
            GetSessionGame().PerformManualStep();
        }

        public void FightAgainstInflation()
        {
            GetSessionGame().FightAgainstInflation();
        }

        public void PlayLottery()
        {
            throw new NotImplementedException();
        }

        public FundsCounters GetCounters()
        {
            return _countersContractMapper.Map(GetSessionGame().FundsCounters);
        }

        public GameData GetGameData()
        {
            var gameData = _gameDataContractMapper.Map(GetSessionGame());
            return gameData;
        }

        public void ActivateDelayedCounter(int counterId)
        {
            GetSessionGame().ActivateDelayedCounter(counterId);
        }

        private IGame GetSessionGame()
        {
            var sessionId = OperationContextHelper.Instance.SessionId ?? default(Guid);
            return _gameSessions.GetGame(sessionId);
        }
    }
}
