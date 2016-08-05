﻿using System;
using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Repository;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.CustomRule;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCustomRuleInfo;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserRating;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class GameDataRetrieveDataAccessService : IGameDataRetrieveDataAccessService
    {
        private readonly IFundsDriverRepository _fundsDriverRepository;
        private readonly ICountersRepository _countersRepository;
        private readonly ICustomRuleRepository _customRuleRepository;
        private readonly IUserGameCounterRepository _userGameCounterRepository;
        private readonly IUserCounterRepository _userCounterRepository;
        private readonly IUserFundsDriverRepository _userFundsDriverRepository;
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IUserCustomRuleRepository _userCustomRuleRepository;

        public GameDataRetrieveDataAccessService(
            IFundsDriverRepository fundsDriverRepository,
            ICountersRepository countersRepository,
            ICustomRuleRepository customRuleRepository,
            IUserGameCounterRepository userGameCounterRepository,
            IUserCounterRepository userCounterRepository,
            IUserFundsDriverRepository userFundsDriverRepository,
            IUserRatingRepository userRatingRepository,
            IUserCustomRuleRepository userCustomRuleRepository)
        {
            _fundsDriverRepository = fundsDriverRepository;
            _countersRepository = countersRepository;
            _customRuleRepository = customRuleRepository;
            _userGameCounterRepository = userGameCounterRepository;
            _userCounterRepository = userCounterRepository;
            _userFundsDriverRepository = userFundsDriverRepository;
            _userRatingRepository = userRatingRepository;
            _userCustomRuleRepository = userCustomRuleRepository;
        }

        public UserRating[] GetUserRatings()
        {
            return _userRatingRepository.GetAllUserRatings(new GetAllUsersRatingsCriterion()).ToArray();
        }

        public GameData GetGameData(int userId)
        {
            var fundsDrivers = _fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion());
            var counters = _countersRepository.FindAll(new GetAllCountersCriterion());
            var customRules = _customRuleRepository.FindAll(new GetAllCustomRulesCriterion());
            var userGameCounters = _userGameCounterRepository.FindById(new GetUserGameCounterByIdCriterion(userId));
            var userCounters = _userCounterRepository.FindByUserId(new GetUserCountersByUserIdCriterion(userId));
            if (userCounters != null)
            {
                foreach (var userCounter in userCounters)
                {
                    var originalCounter = counters.SingleOrDefault(_ => _.Id == userCounter.Id);
                    if (originalCounter == null) continue;
                    var indexOfOriginalCounter = Array.IndexOf(counters, originalCounter);
                    counters[indexOfOriginalCounter] = userCounter;
                }
            }
            var userFundsDrivers =
                _userFundsDriverRepository.FindByUserId(new GetUserFundsDriverByUserIdCriterion(userId));
            if (userFundsDrivers != null)
            {
                foreach (var userFundsDriver in userFundsDrivers)
                {
                    var originalFundDriver = fundsDrivers.SingleOrDefault(_ => _.Id == userFundsDriver.Id);
                    if (originalFundDriver == null) continue;
                    originalFundDriver.BuyCount = userFundsDriver.BuyCount;
                    originalFundDriver.Value = userFundsDriver.Value;
                }
            }
            var userCustomRuleCounters =
                _userCustomRuleRepository.FindByUserId(new GetUserCustomRuleInfoByUserIdCriterion(userId));
            if (userCustomRuleCounters != null)
            {
                foreach (var userCustomRuleCounter in userCustomRuleCounters)
                {
                    var originalFundDriver =
                        fundsDrivers.SingleOrDefault(_ => _.Id == userCustomRuleCounter.FundsDriverId);
                    if (originalFundDriver == null) continue;
                    originalFundDriver.CustomRuleInfo.CurrentIndex = userCustomRuleCounter.CurrentIndex;
                    originalFundDriver.CustomRuleInfo.FundsDriverId = userCustomRuleCounter.FundsDriverId;
                }
            }
            return new GameData()
            {
                FundsDrivers = fundsDrivers,
                Counters = new FundsCounters()
                {
                    Counters = counters,
                    CurrentFunds = userGameCounters != null ? userGameCounters.CurrentFunds : 100,
                    TotalFunds = userGameCounters != null ? userGameCounters.TotalFunds : 100
                },
                CustomRules = customRules,
                AutomaticStepsCount = userGameCounters != null ? userGameCounters.AutomaticStepsCount : 0,
                ManualStepsCount = userGameCounters != null ? userGameCounters.ManualStepsCount : 0
            };
        }
    }
}
