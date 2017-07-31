namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer
{
    using EntityFX.Gdcame.Infrastructure.Common;

    public class RatingServerDatabasesProvider : IDatabasesProvider
    {

        private readonly string providerName;

        private readonly string mongoConnectionStringTemplate = "mongodb://{0}:27017/gdcame";


        public RatingServerDatabasesProvider(string providerName)
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
                    return null; //new EntityFX.Gdcame.DataAccess.Repository.Ef.RatingServer.ContainerBootstrapper();
                case "Mongo":
                    return new EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer.ContainerBootstrapper(connectionString);
                case "LocalStorage":
                default:
                    return null; //new EntityFX.Gdcame.DataAccess.Repository.LocalStorage.RatingServer.ContainerBootstrapper();
            }
        }
    }
}
