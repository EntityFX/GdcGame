namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer
{
    using System.Collections.Generic;

    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.RatingServer;
    using EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer.Mappers;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Microsoft.Practices.Unity;

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
           

            container.RegisterType<IMapper<TopRatingStatistics, List<TopRatingStatisticsDocument>>, TopRatingStatisticsDocumentMapper>();

            container.RegisterType<IGlobalRatingRepository, GlobalRatingRepository>();
            container.RegisterInstance<IMongoDatabase>(this.MongoDatabase);
            return container;
        }
    }
}
