using EntityFX.Gdcame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Repository.Mongo;
using MongoDB.Driver;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class DatabasesProvider : IDatabasesProvider
    {

        private readonly string providerName;

        private readonly string mongoConnectionStringTemplate = "mongodb://{0}:27017/gdcame";

        private Dictionary<string, UserRepository> userRepositories = new Dictionary<string, UserRepository>();

        public DatabasesProvider(string providerName)
        {
            this.providerName = providerName;
        }

        public DatabasesProvider()
        {
            providerName = ConfigurationManager.AppSettings["RepositoryProvider"];
        }

        private UserRepository getUserRepository(string serverId)
        {
            IContainerBootstrapper tempBootstrapper = CreateBootstrapper(serverId);
            //todo:add for other databases
            if (tempBootstrapper is EntityFX.Gdcame.DataAccess.Repository.Mongo.ContainerBootstrapper)
            {
                EntityFX.Gdcame.DataAccess.Repository.Mongo.ContainerBootstrapper mongoBootstrapper =
                    (EntityFX.Gdcame.DataAccess.Repository.Mongo.ContainerBootstrapper)tempBootstrapper;
                IMongoDatabase mongoDatabase = mongoBootstrapper.MongoDatabase;
                UserRepository userRepository = new UserRepository(mongoDatabase);
                return userRepository;
            }
            throw new Exception(String.Format("Bootstrapper is not found for server {0}",
                    serverId));

        }

        private IContainerBootstrapper CreateBootstrapper(string serverId)
        {
            string connectionStringTemplate = "";

            //todo:for other databases
            if ("Mongo".Equals(providerName))
            {
                connectionStringTemplate = mongoConnectionStringTemplate;
            }

            string connectionString = string.Format(connectionStringTemplate, serverId);
            IContainerBootstrapper bootstrapper = GetRepositoryProvider(connectionString);

            return bootstrapper;
        }

        public IContainerBootstrapper GetRepositoryProvider(string connectionString)
        {
            switch (providerName)
            {
                case "EntityFramework":
                    return new DataAccess.Repository.Ef.ContainerBootstrapper();
                case "Mongo":
                    return new DataAccess.Repository.Mongo.ContainerBootstrapper(connectionString);
                case "LocalStorage":
                default:
                    return new DataAccess.Repository.LocalStorage.ContainerBootstrapper();
            }
        }
    }
}
