using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using System;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Manager.Mappers;
using FundsCounters = EntityFX.EconomicsArcade.Contract.Common.Counters.FundsCounters;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using BuyFundDriverResult = EntityFX.EconomicsArcade.Contract.Manager.GameManager.BuyFundDriverResult;

namespace EntityFX.EconomicsArcade.Manager
{
    public class GameManager : IGameManager
    {
        private readonly GameSessions _gameSessions;
        private ILogger _logger;
        private readonly IMapper<IGame, GameData> _gameDataContractMapper;
        private readonly IMapper<Contract.Game.Counters.FundsCounters, FundsCounters> _countersContractMapper;
        private readonly IMapper<Contract.Game.Funds.FundsDriver, FundsDriver> _fundsDriverContractMapper;
        private readonly IMapper<Contract.Game.ManualStepResult, Contract.Manager.GameManager.ManualStepResult> _manualStepResultMapper;

        public GameManager(ILogger logger, GameSessions gameSessions
            , IMapper<IGame, GameData> gameDataContractMapper
            , IMapper<Contract.Game.Counters.FundsCounters, FundsCounters> countersContractMapper
            , IMapper<Contract.Game.Funds.FundsDriver, FundsDriver> fundsDriverContractMapper
            , IMapper<Contract.Game.ManualStepResult, Contract.Manager.GameManager.ManualStepResult> manualStepResultMapper)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _gameDataContractMapper = gameDataContractMapper;
            _countersContractMapper = countersContractMapper;
            _fundsDriverContractMapper = fundsDriverContractMapper;
            _manualStepResultMapper = manualStepResultMapper;
        }

        public BuyFundDriverResult BuyFundDriver(int fundDriverId)
        {
            _logger.Trace("{0}.BuyFundDriver [fundDriverId={1}]", GetType().FullName, fundDriverId);

            var buyFundDriverResult = GetSessionGame().BuyFundDriver(fundDriverId);
            if (buyFundDriverResult != null)
            {
                return new BuyFundDriverResult()
                {
                    ModifiedFundsCounters = _countersContractMapper.Map(buyFundDriverResult.ModifiedFundsCounters),
                    ModifiedFundsDriver = _fundsDriverContractMapper.Map(buyFundDriverResult.ModifiedFundsDriver)
                };
            }
            return null;
        }

        public Contract.Manager.GameManager.ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult)
        {
            _logger.Trace("EntityFX.EconomicsArcade.Manager.GameManager.PerformManualStep():");

            var res = GetSessionGame().PerformManualStep(verificationManualStepResult != null 
                ? new VerificationManualStepData() { ResultNumber = verificationManualStepResult.VerificationNumber} : null);
            return _manualStepResultMapper.Map(res);
        }

        public void FightAgainstInflation()
        {
            _logger.Trace("EntityFX.EconomicsArcade.Manager.GameManager.FightAgainstInflation():");

            try
            {
                GetSessionGame().FightAgainstInflation();
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
            }
        }

        public void PlayLottery()
        {
            _logger.Trace("EntityFX.EconomicsArcade.Manager.GameManager.PlayLottery():");

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
            _logger.Trace("EntityFX.EconomicsArcade.Manager.GameManager.ActivateDelayedCounter():");
            _logger.Info("counterId is {0}", counterId);

            try
            {
                GetSessionGame().ActivateDelayedCounter(counterId);
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
            }
        }

        private IGame GetSessionGame()
        {
            
            var sessionId = OperationContextHelper.Instance.SessionId ?? default(Guid);
            return _gameSessions.GetGame(sessionId);
        }
    }
}
