﻿using System;
using System.ServiceModel;
using System.Web;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Model.Common.Model;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Factories;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Providers
{
    public class GameDataProvider : IGameDataProvider
    {
        private Guid GameGuid { get; set; }
        private ILogger _logger;
        private IGameManager _gameManager;
        private readonly ISimpleUserManager _simpleUserManager;
        private readonly SessionManagerClient _sessionManagerClient;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;
        private readonly IMapper<FundsCounters, FundsCounterModel> _fundsCounterModelMapper;
        private readonly IMapper<BuyFundDriverResult, BuyDriverModel> _fundsDriverBuyinfoModelMapper;
        private readonly IGameClientFactory _gameClientFactory;

        public GameDataProvider(
            ILogger logger,
            IGameClientFactory gameClientFactory,
            ISimpleUserManager simpleUserManager,
            SessionManagerClient sessionManagerClient,
            IMapper<GameData, GameDataModel> gameDataModelMapper,
            IMapper<FundsCounters, FundsCounterModel> fundsCounterModelMapper,
            IMapper<BuyFundDriverResult, BuyDriverModel> fundsDriverBuyinfoModelMapper
            )
        {
            _logger = logger;
            _simpleUserManager = simpleUserManager;
            _sessionManagerClient = sessionManagerClient;
            _gameDataModelMapper = gameDataModelMapper;
            _fundsCounterModelMapper = fundsCounterModelMapper;
            _fundsDriverBuyinfoModelMapper = fundsDriverBuyinfoModelMapper;
            _gameClientFactory = gameClientFactory;
        }

        public void Initialize(string userName)
        {
            //if (HttpContext.Current.Session["SessionGuid"] == null)
            //{
            //    if (!_simpleUserManager.Exists(userName))
            //    {
            //        _simpleUserManager.Create(userName);
            //    }

            //    HttpContext.Current.Session["SessionGuid"] = _sessionManagerClient.AddSession(userName);

            //}
            //GameGuid = (Guid)HttpContext.Current.Session["SessionGuid"];
            //_gameManager = _gameClientFactory.BuildGameClient(GameGuid);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //try
            //{
            //    if (HttpContext.Current.Session["SessionGuid"] == null)
            //    {
            //        if (!_simpleUserManager.Exists(userName))
            //        {
            //            _simpleUserManager.Create(userName);
            //        }
            //        HttpContext.Current.Session["SessionGuid"] = _sessionManagerClient.AddSession(userName);
            //    }

            //    HttpContext.Current.Session["SessionGuid"] = _sessionManagerClient.AddSession(userName);
            //    GameGuid = (Guid)HttpContext.Current.Session["SessionGuid"];
            //    _gameManager = _gameClientFactory.BuildGameClient(GameGuid);

            //    _gameManager.GetGameData();
            //}
            //catch (FaultException exception)
            //{
            //    if (!_simpleUserManager.Exists(userName))
            //    {
            //        _simpleUserManager.Create(userName);
            //    }

            //    HttpContext.Current.Session["SessionGuid"] = _sessionManagerClient.AddSession(userName);
            //    GameGuid = (Guid)HttpContext.Current.Session["SessionGuid"];
            //    _gameManager = _gameClientFactory.BuildGameClient(GameGuid);
            //}
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            if (HttpContext.Current.Session["SessionGuid"] == null)
            {
                if (!_simpleUserManager.Exists(userName))
                {
                    _simpleUserManager.Create(userName);
                }

                HttpContext.Current.Session["SessionGuid"] = _sessionManagerClient.AddSession(userName);

            }
            GameGuid = (Guid)HttpContext.Current.Session["SessionGuid"];
            _gameManager = _gameClientFactory.BuildGameClient(GameGuid);

            try
            {
                _gameManager.GetGameData();
            }
            catch (FaultException exception)
            {
                HttpContext.Current.Session["SessionGuid"] = null;
                Initialize(userName);
            }
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
    }
}