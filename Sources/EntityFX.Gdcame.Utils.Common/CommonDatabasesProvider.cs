namespace EntityFX.Gdcame.Utils.Common
{
    using EntityFX.Gdcame.Infrastructure.Common;

    public class CommonDatabasesProvider : IDatabasesProvider
    {

        private readonly string providerName;

        public CommonDatabasesProvider(string providerName)
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
                    return new DataAccess.Repository.Ef.Common.ContainerBootstrapper();
                case "Mongo":
                    return new EntityFX.Gdcame.DataAccess.Repository.Mongo.Common.ContainerBootstrapper(connectionString);
                case "LocalStorage":
                default:
                    return new DataAccess.Repository.LocalStorage.Common.ContainerBootstrapper();
            }
        }
    }
}
