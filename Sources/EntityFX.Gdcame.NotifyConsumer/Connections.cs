using System.Collections.Generic;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.NotifyConsumer
{
    public class Connections : IConnections
    {
        public Connections()
        {
            ActiveConnections = new Dictionary<string, List<string>>();
        }
        
        public Dictionary<string, List<string>> ActiveConnections { get; private set; }
    }
}