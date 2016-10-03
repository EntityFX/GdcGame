using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class CountersRepository : ICountersRepository
    {

        private IMongoDatabase Database
        {
            get; set;
        }

        public CountersRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public CounterBase[] FindAll(GetAllCountersCriterion criterion)
        {
            IMongoCollection<CounterBase> counterCollection = Database.GetCollection<CounterBase>("Counter");

            var filter = new BsonDocument();
            if (counterCollection.Find<CounterBase>(filter).Count() == 0)
            {
                CounterBase[] counters = new CounterBase[] {
                new SingleCounter() { Id =  0, Name = "GDC Points", Value = 0, Type = 0 },
                new GenericCounter() {Id = 1, Name = "Профессионализм", Type = 1, Value = 0, SubValue = 10, InflationIncreaseSteps = 1000 },
                new GenericCounter() {Id = 2, Name = "Зарплата", Type = 1, Value = 0, UseInAutoSteps = true, InflationIncreaseSteps = 2000},
                new DelayedCounter() {Id = 3, Name = "Квартальный план", Type = 2, Value = 5000000, MiningTimeSeconds = 240  },
                };
                counterCollection.InsertMany(counters);
            }

            return counterCollection.Find(filter).ToList().ToArray();
        }
    }
}
