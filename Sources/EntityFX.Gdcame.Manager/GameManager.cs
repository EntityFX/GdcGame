using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using ManualStepResult = EntityFX.Gdcame.GameEngine.Contract.ManualStepResult;

namespace EntityFX.Gdcame.Manager
{
    public class GameManager : IGameManager
    {
        private readonly IMapper<GameCash, Cash> _countersContractMapper;
        private readonly IMapper<Item, Common.Contract.Items.Item> _fundsDriverContractMapper;
        private readonly IMapper<IGame, GameData> _gameDataContractMapper;
        private readonly GameSessions _gameSessions;
        private readonly IMapper<ManualStepResult, Contract.GameManager.ManualStepResult> _manualStepResultMapper;

        private readonly IMapperFactory _mapperFactory;
        private readonly IOperationContextHelper _operationContextHelper;
        private readonly ILogger _logger;

        public GameManager(ILogger logger, IOperationContextHelper operationContextHelper, GameSessions gameSessions
            , IMapperFactory mapperFactory
            )
        {
            _logger = logger;
            _operationContextHelper = operationContextHelper;
            _gameSessions = gameSessions;

            _mapperFactory = mapperFactory;

            _gameDataContractMapper = _mapperFactory.Build<IGame, GameData>();
            _countersContractMapper = _mapperFactory.Build<GameCash, Cash>();
            _fundsDriverContractMapper = _mapperFactory.Build<Item, Common.Contract.Items.Item>();
            _manualStepResultMapper = _mapperFactory.Build<ManualStepResult, Contract.GameManager.ManualStepResult>();
        }

        public BuyFundDriverResult BuyFundDriver(int fundDriverId)
        {
            var buyFundDriverResult = GetSessionGame().BuyFundDriver(fundDriverId);
            if (buyFundDriverResult != null)
            {
                return new BuyFundDriverResult
                {
                    ModifiedCash = _countersContractMapper.Map(buyFundDriverResult.ModifiedGameCash),
                    ModifiedItem = _fundsDriverContractMapper.Map(buyFundDriverResult.ModifiedItem)
                };
            }
            return null;
        }

        public Contract.GameManager.ManualStepResult PerformManualStep(
            VerificationManualStepResult verificationManualStepResult)
        {
            var res = GetSessionGame().PerformManualStep(verificationManualStepResult != null
                ? new VerificationManualStepData {ResultNumber = verificationManualStepResult.VerificationNumber}
                : null);
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

        public Cash GetCounters()
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
                throw new InvalidSessionException("Game session is not found", sessionId);
            }
            return game;
        }
    }
}