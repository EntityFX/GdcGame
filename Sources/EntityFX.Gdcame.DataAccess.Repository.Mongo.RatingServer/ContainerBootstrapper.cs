namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer
{
    using System.Collections.Generic;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.RatingServer;
    using EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer.Mappers;
    using EntityFX.Gdcame.Infrastructure.Common;

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
           

            container.RegisterType<IMapper<TopRatingStatistics, List<TopRatingStatisticsDocument>>, TopRatingStatisticsDocumentMapper>();

            container.RegisterType<IGlobalRatingRepository, GlobalRatingRepository>();
            container.RegisterType<IMongoDatabase, IMongoDatabase>((scope) => this.MongoDatabase, ContainerScope.Singleton);
            return container;
        }
    }
}
