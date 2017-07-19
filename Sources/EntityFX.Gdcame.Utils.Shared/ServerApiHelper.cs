using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.Utils.Shared
{
    public class ServerApiHelper
    {
        public static string[] GetServers(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (string[])serializer.Deserialize(file, typeof(string[]));
            }
        }

        public static string[] GetServers()
        {
            return GetServers("servers.json");
        }
    }
}
