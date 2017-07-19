namespace EntityFX.Gdcame.Utils.Common
{
    using System.Configuration;

    public class AppConfiguration
    {
        public string RepositoryProvider { get; set; }

        public string WebServer { get; set; }

        public int KestrelThreads { get; set; }

        public string MongoConnectionString { get; set; }

        public int WebApiPort { get; set; }
        public int SignalRPort { get; set; }

        public AppConfiguration()
        {
            MongoConnectionString = ConfigurationManager.AppSettings["MongoConnectionString"];
            RepositoryProvider = ConfigurationManager.AppSettings["RepositoryProvider"] ?? "Mongo";
            WebApiPort = int.Parse(ConfigurationManager.AppSettings["WebApiPort"] ?? "80");
            SignalRPort = int.Parse(ConfigurationManager.AppSettings["WebApiPort"] ?? "80");
            WebServer = ConfigurationManager.AppSettings["WebServer"] ?? "Kestrel";
            KestrelThreads = int.Parse(ConfigurationManager.AppSettings["KestrelThreads"] ?? "32");
        }
    }
}