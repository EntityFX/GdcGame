namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using System;

    using EntityFX.Gdcame.Common.Contract.Counters;
    using EntityFX.Gdcame.Common.Contract.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.CustomRule;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Newtonsoft.Json;

    public abstract class GameDataRetrieveDataAccessBase
    {
        private readonly ICache _cache;
        private readonly ICountersRepository _countersRepository;
        private readonly ICustomRuleRepository _customRuleRepository;
        private readonly IItemRepository _fundsDriverRepository;
        private object _stdLock = new object();
        private static object _lock;

        public GameDataRetrieveDataAccessBase(ICache cache, GameRepositoryFacade gameRepositoryFacade)
        {
            this._cache = cache;
            this._fundsDriverRepository = gameRepositoryFacade.FundsDriverRepository;
            this._countersRepository = gameRepositoryFacade.CountersRepository;
            this._customRuleRepository = gameRepositoryFacade.CustomRuleRepository;
        }


        protected Item[] GetFundDrivers()
        {
            if (!this._cache.Contains("FundDrivers"))
            {
                this._cache.Set("FundDrivers"
                    , Serialize(this._fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion()))
                    , new DateTimeOffset(DateTime.Now.AddDays(1)));
            }
            return Deserialize<Item[]>((string)this._cache.Get("FundDrivers"));
        }

        protected CounterBase[] GetCounters()
        {
            if (!this._cache.Contains("Counters"))
            {
                this._cache.Set("Counters"
                    , Serialize(this._countersRepository.FindAll(new GetAllCountersCriterion()))
                    , new DateTimeOffset(DateTime.Now.AddDays(1)));
            }
            return Deserialize<CounterBase[]>((string)this._cache.Get("Counters"));
        }

        protected CustomRule[] GetCsutomRules()
        {
            if (!this._cache.Contains("CustomRules"))
            {
                this._cache.Set("CustomRules"
                    , Serialize(this._customRuleRepository.FindAll(new GetAllCustomRulesCriterion()))
                    , new DateTimeOffset(DateTime.Now.AddDays(1)));
            }
            return Deserialize<CustomRule[]>((string)this._cache.Get("CustomRules"));
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
}