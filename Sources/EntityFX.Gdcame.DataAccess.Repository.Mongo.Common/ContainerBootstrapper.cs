namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.Common
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Microsoft.Practices.Unity;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.IdGenerators;
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
            BsonClassMap.RegisterClassMap<Server>(
   cm =>
   {
       cm.AutoMap();
       cm.SetIdMember(
                        cm.GetMemberMap(x => x.Address).SetIdGenerator(StringObjectIdGenerator.Instance));
   });


            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IServerRepository, ServerRepository>();
            container.RegisterType<IMongoDatabase, IMongoDatabase>(() =>this.MongoDatabase, ContainerScope.Singleton);
            return container;
        }
    }
}
