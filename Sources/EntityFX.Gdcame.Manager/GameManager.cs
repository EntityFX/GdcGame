using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using BuyFundDriverResult = EntityFX.Gdcame.Manager.Contract.GameManager.BuyFundDriverResult;
using FundsDriver = EntityFX.Gdcame.GameEngine.Contract.Funds.FundsDriver;
using ManualStepResult = EntityFX.Gdcame.GameEngine.Contract.ManualStepResult;

namespace EntityFX.Gdcame.Manager
{
    public class GameManager : IGameManager
    {
        private readonly GameSessions _gameSessions;
        private ILogger _logger;
        private readonly IOperationContextHelper _operationContextHelper;
        private readonly IMapper<IGame, GameData> _gameDataContractMapper;
        private readonly IMapper<FundsCounters, Common.Contract.Counters.FundsCounters> _countersContractMapper;
        private readonly IMapper<FundsDriver, Common.Contract.Funds.FundsDriver> _fundsDriverContractMapper;
        private readonly IMapper<ManualStepResult, Contract.GameManager.ManualStepResult> _manualStepResultMapper;

        public GameManager(ILogger logger, IOperationContextHelper operationContextHelper,GameSessions gameSessions
            , IMapper<IGame, GameData> gameDataContractMapper
            , IMapper<FundsCounters, Common.Contract.Counters.FundsCounters> countersContractMapper
            , IMapper<FundsDriver, Common.Contract.Funds.FundsDriver> fundsDriverContractMapper
            , IMapper<ManualStepResult, Contract.GameManager.ManualStepResult> manualStepResultMapper)
        {
            _logger = logger;
            _operationContextHelper = operationContextHelper;
            _gameSessions = gameSessions;
            _gameDataContractMapper = gameDataContractMapper;
            _countersContractMapper = countersContractMapper;
            _fundsDriverContractMapper = fundsDriverContractMapper;
            _manualStepResultMapper = manualStepResultMapper;
        }

        public BuyFundDriverResult BuyFundDriver(int fundDriverId)
        {
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

        public Contract.GameManager.ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult)
        {
            var res = GetSessionGame().PerformManualStep(verificationManualStepResult != null 
                ? new VerificationManualStepData() { ResultNumber = verificationManualStepResult.VerificationNumber} : null);
            return _manualStepResultMapper.Map(res);
        }

        public void FightAgainstInflation()
        {
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
            throw new NotImplementedException();
        }

        public Common.Contract.Counters.FundsCounters GetCounters()
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
            try
            {
                GetSessionGame().ActivateDelayedCounter(counterId);
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
            }
        }

        public bool Ping()
        {
            GetSessionGame();
            return true;
        }

        private IGame GetSessionGame()
        {

            var sessionId = _operationContextHelper.Instance.SessionId ?? default(Guid);
            IGame game = null;
            try
            {
                game = _gameSessions.GetGame(sessionId);
            }
            catch (InvalidSessionException invalidSessionException)
            {
                _logger.Error(invalidSessionException);
                throw;
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
                throw;
            }
            return game;
        }
    }
}
