using System;
using System.Linq;
using System.Runtime.Caching;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Funds;
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
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserRating;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public abstract class GameDataRetrieveDataAccessBase
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private object _stdLock = new object();
        private readonly IFundsDriverRepository _fundsDriverRepository;
        private readonly ICountersRepository _countersRepository;
        private readonly ICustomRuleRepository _customRuleRepository;

        public GameDataRetrieveDataAccessBase(
            IFundsDriverRepository fundsDriverRepository,
            ICountersRepository countersRepository,
            ICustomRuleRepository customRuleRepository)
        {
            _fundsDriverRepository = fundsDriverRepository;
            _countersRepository = countersRepository;
            _customRuleRepository = customRuleRepository;
        }


        protected FundsDriver[] GetFundDrivers()
        {
            if (!_cache.Contains("FundDrivers"))
            {
                _cache.Set("FundDrivers"
                    , _fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion())
                    , new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) });
            }
            return _cache.Get("FundDrivers") as FundsDriver[];
        }

        protected CounterBase[] GetCounters()
        {
            if (!_cache.Contains("Counters"))
            {
                _cache.Set("Counters"
                    , _countersRepository.FindAll(new GetAllCountersCriterion())
                    , new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) });
            }
            return _cache.Get("Counters") as CounterBase[];
        }

        protected CustomRule[] GetCsutomRules()
        {
            if (!_cache.Contains("CustomRules"))
            {
                _cache.Set("CustomRules"
                    , _customRuleRepository.FindAll(new GetAllCustomRulesCriterion())
                    , new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)) });
            }
            return _cache.Get("CustomRules") as CustomRule[];
        }
    }

    public class GameDataRetrieveDataAccessService : GameDataRetrieveDataAccessBase, IGameDataRetrieveDataAccessService
    {
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
            : base(fundsDriverRepository, countersRepository, customRuleRepository)
        {

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
            var fundsDrivers = (FundsDriver[])GetFundDrivers().Clone();
            var counters = (CounterBase[])GetCounters().Clone();
            var customRules = (CustomRule[])GetCsutomRules().Clone();
            var userGameCounters = _userGameCounterRepository.FindById(new GetUserGameCounterByIdCriterion(userId));
            var userCounters = _userCounterRepository.FindByUserId(new GetUserCountersByUserIdCriterion(userId));
            if (userCounters != null)
            {
                userCounters.AsParallel().ForAll(_ =>
                {
                    var originalCounter = counters.SingleOrDefault(f => f.Id == _.Id);
                    if (originalCounter == null) return;
                    var indexOfOriginalCounter = 0;

                    indexOfOriginalCounter = Array.IndexOf(counters, originalCounter);


                    counters[indexOfOriginalCounter] = _;
                });

            }
            var userFundsDrivers =
                _userFundsDriverRepository.FindByUserId(new GetUserFundsDriverByUserIdCriterion(userId));
            if (userFundsDrivers != null)
            {
                userFundsDrivers.AsParallel().ForAll(_ =>
                {
                    var originalFundDriver = fundsDrivers.SingleOrDefault(f => f.Id == _.Id);
                    if (originalFundDriver == null) return;
                    originalFundDriver.BuyCount = _.BuyCount;
                    originalFundDriver.Value = _.Value;
                });
            }
            var userCustomRuleCounters =
                _userCustomRuleRepository.FindByUserId(new GetUserCustomRuleInfoByUserIdCriterion(userId));
            if (userCustomRuleCounters != null)
            {
                userCustomRuleCounters.AsParallel().ForAll(_ =>
                {
                    var originalFundDriver =
  fundsDrivers.SingleOrDefault(cr => cr.Id == _.FundsDriverId);
                    if (originalFundDriver == null) return;
                    originalFundDriver.CustomRuleInfo.CurrentIndex = _.CurrentIndex;
                    originalFundDriver.CustomRuleInfo.FundsDriverId = _.FundsDriverId;
                });

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

    public class GameDataRetrieveDataAccessDocumentService : GameDataRetrieveDataAccessBase, IGameDataRetrieveDataAccessService
    {
        private readonly IUserGameSnapshotRepository _userGameSnapshotRepository;

        public GameDataRetrieveDataAccessDocumentService(IUserGameSnapshotRepository userGameSnapshotRepository, IFundsDriverRepository fundsDriverRepository,
    ICountersRepository countersRepository,
    ICustomRuleRepository customRuleRepository)
            : base(fundsDriverRepository, countersRepository, customRuleRepository)
        {
            _userGameSnapshotRepository = userGameSnapshotRepository;

        }

        public GameData GetGameData(int userId)
        {
            var userGameData = _userGameSnapshotRepository.FindByUserId(new GetUserGameSnapshotByIdCriterion(userId));
            if (userGameData == null)
            {
                var fundsDrivers = (FundsDriver[])GetFundDrivers().Clone();
                var counters = (CounterBase[])GetCounters().Clone();
                var customRules = (CustomRule[])GetCsutomRules().Clone();
                return new GameData()
                {
                    FundsDrivers = fundsDrivers,
                    Counters = new FundsCounters()
                    {
                        Counters = counters,
                        CurrentFunds = 100,
                        TotalFunds = 100
                    },
                    CustomRules = customRules,
                    AutomaticStepsCount =  0,
                    ManualStepsCount =  0
                };
            }
            return userGameData;
        }

        public UserRating[] GetUserRatings()
        {
            return new UserRating[0];
        }
    }
}
