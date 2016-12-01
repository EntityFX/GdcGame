using EntityFX.Gdcame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        private readonly string _mongoConnectionString;
        private MongoClient _mongoClient;
        
        public IMongoDatabase MongoDatabase
        {
            get
            {
                if (_mongoClient == null)
                    _mongoClient = new MongoClient(_mongoConnectionString);
                    return _mongoClient.GetDatabase(MongoUrl.Create(_mongoConnectionString).DatabaseName);
            }
        }

        public ContainerBootstrapper(string mongoConnectionString)
        {
            _mongoConnectionString = mongoConnectionString;
        }

        public IUnityContainer Configure(IUnityContainer container)
        {
            ConventionRegistry.Register(
            "DictionaryRepresentationConvention",
            new ConventionPack { new DictionaryRepresentationConvention(DictionaryRepresentation.ArrayOfArrays) },
            _ => true);

            BsonClassMap.RegisterClassMap<CounterBase>(cm => {
                cm.AutoMap();
                cm.SetIsRootClass(true);
            });

            BsonClassMap.RegisterClassMap<SingleCounter>();
            BsonClassMap.RegisterClassMap<DelayedCounter>();
            BsonClassMap.RegisterClassMap<GenericCounter>();


            BsonClassMap.RegisterClassMap<StoredCounterBase>(cm => {
                cm.AutoMap();
                cm.SetIsRootClass(true);
            });


            BsonClassMap.RegisterClassMap<StoredGameDataWithUserId>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(x => x.UserId)
                  .SetIdGenerator(StringObjectIdGenerator.Instance));
            });

            BsonClassMap.RegisterClassMap<RatingStatistics>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(x => x.UserId)
                  .SetIdGenerator(StringObjectIdGenerator.Instance));
            });

            BsonClassMap.RegisterClassMap<StoredGenericCounter>();
            BsonClassMap.RegisterClassMap<StoredSingleCounter>();
            BsonClassMap.RegisterClassMap<StoredDelayedCounter>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();         
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            container.RegisterType<IRatingHistoryRepository, RatingHistoryRepository>();
            container.RegisterType<IRatingStatisticsRepository, RatingStatisticsRepository>();
            container.RegisterInstance<IMongoDatabase>(MongoDatabase);
            return container;
        }
    }
}
