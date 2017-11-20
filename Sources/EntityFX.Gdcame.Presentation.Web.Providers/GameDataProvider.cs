using System;
using System.Linq;
using System.Security.Claims;
using EntityFX.Gdcame.Application.Api.Common.Providers;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;
using Microsoft.AspNetCore.Http;

namespace EntityFX.Gdcame.Application.Providers.MainServer
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;

    public class GameDataProvider : IGameDataProvider
    {
        private readonly IMapper<Cash, CashModel> _fundsCounterModelMapper;
        private readonly IMapper<BuyFundDriverResult, BuyItemModel> _fundsDriverBuyinfoModelMapper;
        private readonly IGameClientFactory _gameClientFactory;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;
        private readonly IMapperFactory _mapperFactory;
        private readonly ISessionManagerClientFactory _sessionManagerClient;
        private readonly ISimpleUserManager _simpleUserManager;
        private IGameManager _gameManager;
        private ILogger _logger;

        public GameDataProvider(
            ILogger logger,
            IGameClientFactory gameClientFactory,
            ISimpleUserManager simpleUserManager,
            ISessionManagerClientFactory sessionManagerClient,
            IMapperFactory mapperFactory
            )
        {
            _logger = logger;
            _simpleUserManager = simpleUserManager;
            _sessionManagerClient = sessionManagerClient;

            _mapperFactory = mapperFactory;

            _gameDataModelMapper = _mapperFactory.Build<GameData, GameDataModel>();
            _fundsCounterModelMapper = _mapperFactory.Build<Cash, CashModel>();
            _fundsDriverBuyinfoModelMapper = _mapperFactory.Build<BuyFundDriverResult, BuyItemModel>();
            _gameClientFactory = gameClientFactory;
        }

        private Guid GameGuid { get; set; }

        public void InitializeGameContext(Guid gameGuid)
        {
            GameGuid = gameGuid;
            _gameManager = _gameClientFactory.BuildGameClient(GameGuid);
        }

        public void ClearSession()
        {

        }

        public GameDataModel GetGameData()
        {
            var gameData = _gameDataModelMapper.Map(_gameManager.GetGameData());
            foreach (var fundsDriverModel in gameData.Items)
            {
                fundsDriverModel.IsUnlocked = gameData.Cash.Counters[0].Value >= fundsDriverModel.UnlockBalance;
            }
            return gameData;
        }

        public CashModel GetCounters()
        {
            return _fundsCounterModelMapper.Map(_gameManager.GetCounters());
        }

        public BuyItemModel BuyFundDriver(int id)
        {
            var buyResult = _gameManager.BuyFundDriver(id);
            return buyResult != null ? _fundsDriverBuyinfoModelMapper.Map(buyResult) : null;
        }

        public ManualStepResultModel PerformManualStep(int? verificationNumber)
        {
            var result = _gameManager.PerformManualStep(
                verificationNumber != null
                    ? new VerificationManualStepResult {VerificationNumber = verificationNumber.Value}
                    : null);
            var verificationnumberResult = result as VerificationRequiredResult;
            VerificationData verificationData = null;
            CashModel modifiedCounters = null;
            if (verificationnumberResult != null)
            {
                verificationData = new VerificationData
                {
                    FirstNumber = verificationnumberResult.FirstNumber,
                    SecondNumber = verificationnumberResult.SecondNumber
                };
            }

            var noVerficationRequiredResult = result as NoVerficationRequiredResult;
            if (noVerficationRequiredResult != null)
            {
                modifiedCounters = _fundsCounterModelMapper.Map(noVerficationRequiredResult.ModifiedCash);
                verificationData = null;
            }

            return new ManualStepResultModel
            {
                VerificationData = verificationData,
                ModifiedCountersInfo = modifiedCounters
            };
        }

        public void FightAgainstInflation()
        {
            _gameManager.FightAgainstInflation();
        }

        public void ActivateDelayedCounter(int counterId)
        {
            _gameManager.ActivateDelayedCounter(counterId);
        }
        /*
        public UserRating[] GetUsersRatingByCount(int count)
        {
            return _ratingManager.GetUsersRatingByCount(count);
        }

        public UserRating FindUserRatingByUserName(string userName)
        {
            return _ratingManager.FindUserRatingByUserName(userName);
        }

        public UserRating[] FindUserRatingByUserNameAndAroundUsers(string userName, int count)
        {
            return _ratingManager.FindUserRatingByUserNameAndAroundUsers(userName, count);
        }*/
    }
}