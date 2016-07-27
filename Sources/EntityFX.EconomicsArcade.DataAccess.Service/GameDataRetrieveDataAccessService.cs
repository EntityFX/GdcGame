using System;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.UserRating;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserRating;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class GameDataRetrieveDataAccessService : IGameDataRetrieveDataAccessService
    {
        private readonly IFundsDriverRepository _fundsDriverRepository;
        private readonly ICountersRepository _countersRepository;
        private readonly IUserGameCounterRepository _userGameCounterRepository;
        private readonly IUserCounterRepository _userCounterRepository;
        private readonly IUserFundsDriverRepository _userFundsDriverRepository;
        private readonly IUserRatingRepository _userRatingRepository;

        public GameDataRetrieveDataAccessService(
            IFundsDriverRepository fundsDriverRepository,
            ICountersRepository countersRepository,
            IUserGameCounterRepository userGameCounterRepository,
            IUserCounterRepository userCounterRepository,
            IUserFundsDriverRepository userFundsDriverRepository,
            IUserRatingRepository userRatingRepository)
        {
            _fundsDriverRepository = fundsDriverRepository;
            _countersRepository = countersRepository;
            _userGameCounterRepository = userGameCounterRepository;
            _userCounterRepository = userCounterRepository;
            _userFundsDriverRepository = userFundsDriverRepository;
            _userRatingRepository = userRatingRepository;
        }

        public UserRating[] GetUserRatings()
        {
            return _userRatingRepository.GetAllUserRatings(new GetAllUsersRatingsCriterion()).ToArray();
        }

        public Contract.Common.GameData GetGameData(int userId)
        {
            var fundsDrivers = _fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion());
            var counters = _countersRepository.FindAll(new GetAllCountersCriterion());
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
            return new Contract.Common.GameData()
            {
                FundsDrivers = fundsDrivers,
                Counters = new FundsCounters()
                {
                    Counters = counters,
                    CurrentFunds = userGameCounters != null ? userGameCounters.CurrentFunds : 100,
                    TotalFunds = userGameCounters != null ? userGameCounters.TotalFunds : 100
                },
                AutomaticStepsCount = userGameCounters != null ? userGameCounters.AutomaticStepsCount : 0,
                ManualStepsCount = userGameCounters != null ? userGameCounters.ManualStepsCount : 0
            };
        }
    }
}
