using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Manager.Mappers;
using FundsCounters = EntityFX.EconomicsArcade.Contract.Manager.GameManager.Counters.FundsCounters;

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
            throw new NotImplementedException();
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

        private IGame GetSessionGame()
        {
            var sessionId = OperationContextHelper.Instance.SessionId ?? default(Guid);
            return _gameSessions.GetGame(sessionId);
        }
    }
}
