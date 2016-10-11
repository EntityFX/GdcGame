using System;
using System.Linq;
using System.Runtime.Caching;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.CustomRule;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public abstract class GameDataRetrieveDataAccessBase
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly ICountersRepository _countersRepository;
        private readonly ICustomRuleRepository _customRuleRepository;
        private readonly IItemRepository _fundsDriverRepository;
        private object _stdLock = new object();
        private static object _lock;

        public GameDataRetrieveDataAccessBase(GameRepositoryFacade gameRepositoryFacade)
        {
            _fundsDriverRepository = gameRepositoryFacade.FundsDriverRepository;
            _countersRepository = gameRepositoryFacade.CountersRepository;
            _customRuleRepository = gameRepositoryFacade.CustomRuleRepository;
        }


        protected Item[] GetFundDrivers()
        {
            if (!_cache.Contains("FundDrivers"))
            {
                _cache.Set("FundDrivers"
                    , Serialize(_fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion()))
                    , new CacheItemPolicy {AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1))});
            }
            return Deserialize<Item[]>((string)_cache.Get("FundDrivers"));
        }

        protected CounterBase[] GetCounters()
        {
            if (!_cache.Contains("Counters"))
            {
                _cache.Set("Counters"
                    , Serialize(_countersRepository.FindAll(new GetAllCountersCriterion()))
                    , new CacheItemPolicy {AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1))});
            }
            return Deserialize<CounterBase[]>((string)_cache.Get("Counters"));
        }

        protected CustomRule[] GetCsutomRules()
        {
            if (!_cache.Contains("CustomRules"))
            {
                _cache.Set("CustomRules"
                    , Serialize(_customRuleRepository.FindAll(new GetAllCustomRulesCriterion()))
                    , new CacheItemPolicy {AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1))});
            }
            return Deserialize<CustomRule[]>((string)_cache.Get("CustomRules"));
        }

        private static string Serialize<T>(T obj)
        {
            // In the PCL we do not have the BinaryFormatter
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        private static T Deserialize<T>(string data)
        {
            // In the PCL we do not have the BinaryFormatter
            return JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }

    public class GameDataRetrieveDataAccessDocumentService : GameDataRetrieveDataAccessBase,
        IGameDataRetrieveDataAccessService
    {
        private readonly IUserGameSnapshotRepository _userGameSnapshotRepository;

        public GameDataRetrieveDataAccessDocumentService(IUserGameSnapshotRepository userGameSnapshotRepository,
            GameRepositoryFacade gameRepositoryFacade)
            : base(gameRepositoryFacade)
        {
            _userGameSnapshotRepository = userGameSnapshotRepository;
        }

        public GameData GetGameData(string userId)
        {
            var userGameData = _userGameSnapshotRepository.FindByUserId(new GetUserGameSnapshotByIdCriterion(userId));
            var originalItems = GetFundDrivers();
            var originalCounters = GetCounters();
            var originalCustomRules = GetCsutomRules();
            var cash = new Cash
            {
                Counters = originalCounters,
                OnHand = 100,
                Total = 100
            };
            if (userGameData != null)
            {
                var originalItemsDisct = originalItems.ToDictionary(_ => _.Id, _ => _);
                var originalCountersDisct = originalCounters.ToDictionary(_ => _.Id, _ => _);
                var originalCustomRulesDisct = originalCustomRules.ToDictionary(_ => _.Id, _ => _);
                foreach (var storedItem in userGameData.Items)
                {
                    var originalItem = originalItemsDisct[storedItem.Id];
                    originalItem.Bought = storedItem.Bought;
                    originalItem.Price = storedItem.Price;
                    foreach (var storedIncrementor in storedItem.Incrementors)
                    {
                        originalItem.Incrementors[storedIncrementor.Key].Value = storedIncrementor.Value;
                    }
                }

                for (var index = 0; index < userGameData.Cash.Counters.Length; index++)
                {
                    var storedCounter = userGameData.Cash.Counters[index];
                    var originalCounter = originalCountersDisct[storedCounter.Id];
                    originalCounter.Value = storedCounter.Value;
                    switch (originalCounter.Type)
                    {
                        case 1:
                            var storedGenericCounter = (StoredGenericCounter) storedCounter;
                            var originalGenericCounter = (GenericCounter) originalCounter;
                            originalGenericCounter.BonusPercentage = storedGenericCounter.BonusPercent;
                            originalGenericCounter.CurrentSteps = storedGenericCounter.CurrentSteps;
                            originalGenericCounter.Inflation = storedGenericCounter.Inflation;
                            originalGenericCounter.SubValue = storedGenericCounter.Value;
                            break;
                        case 2:
                            var storedDelayedCounter = (StoredDelayedCounter) storedCounter;
                            var originalDelayedCounter = (DelayedCounter) originalCounter;
                            originalDelayedCounter.SecondsRemaining = storedDelayedCounter.SecondsRemaining;
                            originalCounter.Value = storedDelayedCounter.DelayedValue;
                            break;
                    }
                }
                cash.OnHand = userGameData.Cash.Balance;
                cash.Total = userGameData.Cash.TotalEarned;
            }
            return new GameData
            {
                Items = originalItems,
                Cash = cash,
                CustomRules = originalCustomRules,
                AutomatedStepsCount = 0,
                ManualStepsCount = 0
            };
        }

        public UserRating[] GetUserRatings()
        {
            return new UserRating[0];
        }
    }
}