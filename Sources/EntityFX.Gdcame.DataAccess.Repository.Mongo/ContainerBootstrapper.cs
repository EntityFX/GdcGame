namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer
{
    using EntityFX.Gdcame.Common.Contract.Counters;
    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.Server;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Microsoft.Practices.Unity;

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

            BsonClassMap.RegisterClassMap<Server>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.SetIdMember(
                            cm.GetMemberMap(x => x.Address).SetIdGenerator(StringObjectIdGenerator.Instance));
                    });  

            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();         
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            container.RegisterType<IRatingHistoryRepository, RatingHistoryRepository>();

            container.RegisterType<IServerRepository, ServerRepository>();
            container.RegisterInstance<IMongoDatabase>(this.MongoDatabase);
            return container;
        }
    }
}
