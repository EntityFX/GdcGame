namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage.MainServer
{
    using System.IO;

    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.Counters;

    using Newtonsoft.Json;

    public class CountersRepository : ICountersRepository
    {
        public CounterBase[] FindAll(GetAllCountersCriterion criterion)
        {
            if (!File.Exists(Path.Combine("storage", "counters", "counters.json")))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path.Combine("storage", "counters", "counters.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (CounterBase[])serializer.Deserialize(file, typeof(CounterBase[]));
            }
        }
    }
}
