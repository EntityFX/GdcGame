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
using EntityFX.Gdcame.Presentation.Web.Model;
using EntityFX.Gdcame.Utils.Common;

namespace EntityFX.Gdcame.Presentation.Web.Providers.Providers
{
    public class GameDataProvider : IGameDataProvider
    {
        private Guid GameGuid { get; set; }
        private ILogger _logger;
        private IGameManager _gameManager;
        private readonly ISimpleUserManager _simpleUserManager;
        private readonly ISessionManagerClientFactory _sessionManagerClient;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;
        private readonly IMapper<FundsCounters, FundsCounterModel> _fundsCounterModelMapper;
        private readonly IMapper<BuyFundDriverResult, BuyDriverModel> _fundsDriverBuyinfoModelMapper;
        private readonly IGameClientFactory _gameClientFactory;
        private readonly IRatingManager _ratingManager;

        public GameDataProvider(
            ILogger logger,
            IGameClientFactory gameClientFactory,
            ISimpleUserManager simpleUserManager,
            ISessionManagerClientFactory sessionManagerClient,
            IMapper<GameData, GameDataModel> gameDataModelMapper,
            IMapper<FundsCounters, FundsCounterModel> fundsCounterModelMapper,
            IMapper<BuyFundDriverResult, BuyDriverModel> fundsDriverBuyinfoModelMapper,
            IRatingManager ratingManager
            )
        {
            _logger = logger;
            _simpleUserManager = simpleUserManager;
            _sessionManagerClient = sessionManagerClient;
            _gameDataModelMapper = gameDataModelMapper;
            _fundsCounterModelMapper = fundsCounterModelMapper;
            _fundsDriverBuyinfoModelMapper = fundsDriverBuyinfoModelMapper;
            _gameClientFactory = gameClientFactory;
            _ratingManager = ratingManager;
        }

        public void InitializeSession(string userName)
        {
            if (HttpContext.Current.Session["SessionGuid"] == null)
            {
                if (!_simpleUserManager.Exists(userName))
                {
                    _simpleUserManager.Create(new UserData() {Login = userName});
                }

                HttpContext.Current.Session["SessionGuid"] = _sessionManagerClient.BuildSessionManagerClient(Guid.Empty).OpenSession(userName);

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
            foreach (var fundsDriverModel in gameData.FundsDrivers)
            {
                fundsDriverModel.IsActive = gameData.Counters.Counters[0].Value >= fundsDriverModel.UnlockValue;
            }
            return gameData;
        }

        public FundsCounterModel GetCounters()
        {
            return _fundsCounterModelMapper.Map(_gameManager.GetCounters());
        }

        public BuyDriverModel BuyFundDriver(int id)
        {
            var buyResult = _gameManager.BuyFundDriver(id);
            return buyResult != null ? _fundsDriverBuyinfoModelMapper.Map(buyResult) : null;

        }

        public ManualStepResultModel PerformManualStep(int? verificationNumber)
        {
            var result = _gameManager.PerformManualStep(
                 verificationNumber != null ? new VerificationManualStepResult() { VerificationNumber = verificationNumber.Value } : null);
            var verificationnumberResult = result as VerificationRequiredResult;
            VerificationData verificationData = null;
            FundsCounterModel modifiedCounters = null;
            if (verificationnumberResult != null)
            {
                verificationData = new VerificationData()
                {
                    FirstNumber = verificationnumberResult.FirstNumber,
                    SecondNumber = verificationnumberResult.SecondNumber
                };
            }

            var noVerficationRequiredResult = result as NoVerficationRequiredResult;
            if (noVerficationRequiredResult != null)
            {
                modifiedCounters = _fundsCounterModelMapper.Map(noVerficationRequiredResult.ModifiedCounters);
            }

            return new ManualStepResultModel()
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
        }
    }
}