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
            return new CounterBase[] {
                //new SingleCounter() { Id = 0, Name = "Points", Type = 0, Value = 0 },
                new SingleCounter() { Id =  0, Name = "GDC Points", Value = 0, Type = 0 },
                new GenericCounter() {Id = 1, Name = "Профессионализм", Type = 1, Value = 0, SubValue = 10, InflationIncreaseSteps = 1000 },
                new GenericCounter() {Id = 2, Name = "Зарплата", Type = 1, Value = 0, UseInAutoSteps = true, InflationIncreaseSteps = 2000},
                new DelayedCounter() {Id = 3, Name = "Квартальный план", Type = 2, Value = 5000000, MiningTimeSeconds = 240, UnlockValue = 250  },
            };
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

//        INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES(0, N'GDC Points', 0.00, CONVERT(bit, 'False'), 1000, NULL, 0)
//INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES(1, N'Профессионализм', 10.00, CONVERT(bit, 'False'), 1000, NULL, 1)
//INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES(2, N'Зарплата', 0.00, CONVERT(bit, 'True'), 2000, NULL, 1)
//INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES(3, N'Квартальный план', 5000000.00, CONVERT(bit, 'False'), 1000, 240, 2)

    }
}
