﻿namespace EntityFX.Gdcame.Utils.Common
{
    public class AppConfiguration
    {
        public string RepositoryProvider { get; set; }

        public string MongoConnectionString { get; set; }

        public int WebApiPort { get; set; }
        public int SignalRPort { get; set; }
    }
}