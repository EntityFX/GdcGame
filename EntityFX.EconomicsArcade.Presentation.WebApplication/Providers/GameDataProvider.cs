﻿using System;
using System.Web;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Factories;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Providers
{
    public class GameDataProvider : IGameDataProvider
    {
        public Guid GameGuid { get; private set; }
        private IGameManager _gameManager;
        private readonly ISimpleUserManager _simpleUserManager;
        private readonly SessionManagerClient _sessionManagerClient;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;
        private readonly IGameClientFactory _gameClientFactory;

        public GameDataProvider(
            IGameClientFactory gameClientFactory,
            ISimpleUserManager simpleUserManager,
            SessionManagerClient sessionManagerClient,
            IMapper<GameData, GameDataModel> gameDataModelMapper
            )
        {
            _simpleUserManager = simpleUserManager;
            _sessionManagerClient = sessionManagerClient;
            _gameDataModelMapper = gameDataModelMapper;
            _gameClientFactory = gameClientFactory;
        }

        public void Initialize(string userName)
        {
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
        }

        public GameDataModel GetGameData()
        {
            return _gameDataModelMapper.Map(_gameManager.GetGameData());
        }

        public void BuyFundDriver(int id)
        {
            _gameManager.BuyFundDriver(id);
        }

        public void PerformManualStep()
        {
            _gameManager.PerformManualStep();
        }

        public void FightAgainstInflation()
        {
            _gameManager.FightAgainstInflation();
        }

    }
}