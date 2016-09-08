using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class CountersRepository : ICountersRepository
    {
        public CounterBase[] FindAll(GetAllCountersCriterion criterion)
        {
            return new CounterBase[] {
                new SingleCounter() { Id = 0, Name = "Points", Type = 0, Value = 0 },
                new GenericCounter() {Id = 1, Name = "Production", Type = 1, Value = 0, SubValue = 10 },
                new GenericCounter() {Id = 2, Name = "Rent", Type = 1, Value = 0, SubValue = 10, UseInAutoSteps = true },
                new DelayedCounter() {Id = 3, Name = "Rent", Type = 2, Value = 1000, MiningTimeSeconds = 3600  },
            };
        }
    }
}
