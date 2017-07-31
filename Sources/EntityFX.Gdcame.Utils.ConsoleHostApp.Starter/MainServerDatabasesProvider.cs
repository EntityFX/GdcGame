namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    using EntityFX.Gdcame.Infrastructure.Common;

    public class MainServerDatabasesProvider : IDatabasesProvider
    {

        private readonly string providerName;

        private readonly string mongoConnectionStringTemplate = "mongodb://{0}:27017/gdcame";


        public MainServerDatabasesProvider(string providerName)
        {
            this.providerName = providerName;
        }

        //public DatabasesProvider()
        //{
        //    this.providerName = ConfigurationManager.AppSettings["RepositoryProvider"];
        //}

        public IContainerBootstrapper GetRepositoryProvider(string connectionString)
        {
            switch (this.providerName)
            {
                case "EntityFramework":
                    return new DataAccess.Repository.Ef.MainServer.ContainerBootstrapper();
                case "Mongo":
                    return new EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer.ContainerBootstrapper(connectionString);
                case "LocalStorage":
                default:
                    return new DataAccess.Repository.LocalStorage.MainServer.ContainerBootstrapper();
            }
        }
    }
}
