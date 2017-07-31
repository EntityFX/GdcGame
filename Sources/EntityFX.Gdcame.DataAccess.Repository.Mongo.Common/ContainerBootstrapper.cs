﻿namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.Common
{
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
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

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IRatingStatisticsRepository, RatingStatisticsRepository>();
            container.RegisterType<IServerRepository, ServerRepository>();
            container.RegisterInstance<IMongoDatabase>(this.MongoDatabase);
            return container;
        }
    }
}
