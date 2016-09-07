using System;
using System.Web;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Utils.Common;

namespace EntityFX.Gdcame.Presentation.Web.Providers.Providers
{
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

        public void InitializeSession(string userName)
        {
            if (HttpContext.Current.Session["SessionGuid"] == null)
            {
                if (!_simpleUserManager.Exists(userName))
                {
                    _simpleUserManager.Create(new UserData {Login = userName});
                }

                HttpContext.Current.Session["SessionGuid"] =
                    _sessionManagerClient.BuildSessionManagerClient(Guid.Empty).OpenSession(userName);
            }
            InitializeGameContext((Guid) HttpContext.Current.Session["SessionGuid"]);
        }

        public void InitializeGameContext(Guid gameGuid)
        {
            GameGuid = gameGuid;
            _gameManager = _gameClientFactory.BuildGameClient(GameGuid);
        }

        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
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