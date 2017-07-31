namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer
{
    using EntityFX.Gdcame.Common.Contract.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.Counters;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class CountersRepository : ICountersRepository
    {

        private IMongoDatabase Database
        {
            get; set;
        }

        public CountersRepository(IMongoDatabase database)
        {
            this.Database = database;
        }

        public CounterBase[] FindAll(GetAllCountersCriterion criterion)
        {
            IMongoCollection<CounterBase> counterCollection = this.Database.GetCollection<CounterBase>("Counter");

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

            return counterCollection.Find(filter).SortBy(_ => _.Id).ToList().ToArray();
        }
    }
}
