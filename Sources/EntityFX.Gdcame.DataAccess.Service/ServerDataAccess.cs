using EntityFX.Gdcame.DataAccess.Contract.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class ServerDataAccessService : IServerDataAccessService
    {
        public string[] GetServers()
        {
            if (!File.Exists(Path.Combine("servers.json")))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path.Combine("servers.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (string[])serializer.Deserialize(file, typeof(string[]));
            }
        }
    }
}
