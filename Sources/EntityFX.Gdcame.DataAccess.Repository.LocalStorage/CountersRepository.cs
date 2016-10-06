using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;
using System.IO;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage
{
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
