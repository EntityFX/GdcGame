namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Contract.MainServer.Store;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Bson.Serialization.IdGenerators;
    using MongoDB.Bson.Serialization.Options;
    using MongoDB.Driver;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        private readonly string _mongoConnectionString;
        private MongoClient _mongoClient;
        
        public IMongoDatabase MongoDatabase
        {
            get
            {
                if (this._mongoClient == null)
                    this._mongoClient = new MongoClient(this._mongoConnectionString);
                    return this._mongoClient.GetDatabase(MongoUrl.Create(this._mongoConnectionString).DatabaseName);
            }
        }

        public ContainerBootstrapper(string mongoConnectionString)
        {
            this._mongoConnectionString = mongoConnectionString;
        }

        public IIocContainer Configure(IIocContainer container)
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

            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();         
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            container.RegisterType<IRatingHistoryRepository, RatingHistoryRepository>();
            container.RegisterType<ILocalRatingStatisticsRepository, LocalRatingStatisticsRepository>();
            container.RegisterType<IMongoDatabase, IMongoDatabase>(() => this.MongoDatabase, ContainerScope.Singleton);
            return container;
        }
    }
}
