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

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<IUserRatingRepository, UserRatingRepository>();
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            container.RegisterInstance<IMongoDatabase>(MongoDatabase);
            return container;
        }
    }
}
