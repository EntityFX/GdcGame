using System;

using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;

using ManualStepResult = EntityFX.Gdcame.Kernel.Contract.ManualStepResult;

namespace EntityFX.Gdcame.Manager.MainServer
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    using IGame = EntityFX.Gdcame.Kernel.Contract.IGame;
    using VerificationManualStepData = EntityFX.Gdcame.Kernel.Contract.VerificationManualStepData;

    public class GameManager : IGameManager
    {
        private readonly IMapper<GameCash, Cash> _countersContractMapper;
        private readonly IMapper<Item, Gdcame.Contract.MainServer.Items.Item> _fundsDriverContractMapper;
        private readonly IMapper<IGame, GameData> _gameDataContractMapper;
        private readonly IGameSessions _gameSessions;
        private readonly IMapper<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult> _manualStepResultMapper;

        private readonly IMapperFactory _mapperFactory;
        private readonly IOperationContextHelper _operationContextHelper;
        private readonly ILogger _logger;

        public GameManager(ILogger logger, IOperationContextHelper operationContextHelper, IGameSessions gameSessions
            , IMapperFactory mapperFactory
            )
        {
            _logger = logger;
            _operationContextHelper = operationContextHelper;
            _gameSessions = gameSessions;

            _mapperFactory = mapperFactory;

            _gameDataContractMapper = _mapperFactory.Build<IGame, GameData>();
            _countersContractMapper = _mapperFactory.Build<GameCash, Cash>();
            _fundsDriverContractMapper = _mapperFactory.Build<Item, Gdcame.Contract.MainServer.Items.Item>();
            _manualStepResultMapper = _mapperFactory.Build<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult>();
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

        public Contract.MainServer.GameManager.ManualStepResult PerformManualStep(
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
                //TODO: if Game IsFreezed then throw GameFreezedException() that will be caught by wrapping service and return NeedToChangeNode response to Client.
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