using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EntityFX.Gdcame.NotifyConsumer.Contract
{
    public interface IConnections
    {
        Dictionary<string, List<string>> ActiveConnections { get; }  
    }
}