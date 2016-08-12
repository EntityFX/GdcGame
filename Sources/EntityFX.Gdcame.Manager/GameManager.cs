using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using BuyFundDriverResult = EntityFX.Gdcame.Manager.Contract.GameManager.BuyFundDriverResult;
using ManualStepResult = EntityFX.Gdcame.GameEngine.Contract.ManualStepResult;

namespace EntityFX.Gdcame.Manager
{
    public class GameManager : IGameManager
    {
        private readonly GameSessions _gameSessions;
        private ILogger _logger;
        private readonly IOperationContextHelper _operationContextHelper;
        private readonly IMapper<IGame, GameData> _gameDataContractMapper;
        private readonly IMapper<GameCash, Common.Contract.Counters.Cash> _countersContractMapper;
        private readonly IMapper<Item, Common.Contract.Items.Item> _fundsDriverContractMapper;
        private readonly IMapper<ManualStepResult, Contract.GameManager.ManualStepResult> _manualStepResultMapper;

        public GameManager(ILogger logger, IOperationContextHelper operationContextHelper,GameSessions gameSessions
            , IMapper<IGame, GameData> gameDataContractMapper
            , IMapper<GameCash, Common.Contract.Counters.Cash> countersContractMapper
            , IMapper<Item, Common.Contract.Items.Item> fundsDriverContractMapper
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
                    ModifiedCash = _countersContractMapper.Map(buyFundDriverResult.ModifiedGameCash),
                    ModifiedItem = _fundsDriverContractMapper.Map(buyFundDriverResult.ModifiedItem)
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

        public Common.Contract.Counters.Cash GetCounters()
        {
            return _countersContractMapper.Map(GetSessionGame().GameCash);
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
