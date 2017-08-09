namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.RatingServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    using MongoDB.Driver;

    public class GlobalRatingRepository : IGlobalRatingRepository
    {
        private readonly IMapper<TopRatingStatistics, List<TopRatingStatisticsDocument>> _topRatingStatisticsDocumentsMapper;
        //private readonly IMapper<List<TopRatingStatisticsDocument>, TopRatingStatistics> _topRatingStatisticsMapper;

        private readonly Func<int, int, List<TopRatingStatisticsDocument>> statisticsFindQuery;


        private IMongoDatabase Database
        {
            get; set;
        }
        public GlobalRatingRepository(IMongoDatabase database, IMapperFactory mapperFactory)
            : base()
        {
            this.statisticsFindQuery = (counter, period) =>
            {
                IMongoCollection<TopRatingStatisticsDocument> collection = this.Database.GetCollection<TopRatingStatisticsDocument>("TopRatingStatistics");
                var counterTypeFilter = Builders<TopRatingStatisticsDocument>.Filter.Eq("CounterType", counter);
                var periodTypeFilter = Builders<TopRatingStatisticsDocument>.Filter.Eq("PeriodType", period);
                var commonFilter = Builders<TopRatingStatisticsDocument>.Filter.And(counterTypeFilter, periodTypeFilter);
                var query =
                    collection.Find(commonFilter)
                        .Sort(Builders<TopRatingStatisticsDocument>.Sort.Descending(document => document.Value))
                        .Limit(500);
                return query.ToList();
            };

            this.Database = database;
            this._topRatingStatisticsDocumentsMapper = mapperFactory.Build<TopRatingStatistics, List<TopRatingStatisticsDocument>>();
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
            var counterPeriodCombination = new List<Tuple<int, int>>
                                                {
                                                    new Tuple<int, int>(0, 0),
                                                    new Tuple<int, int>(0, 1),
                                                    new Tuple<int, int>(0, 2),
                                                    new Tuple<int, int>(1, 0),
                                                    new Tuple<int, int>(1, 1),
                                                    new Tuple<int, int>(1, 2),
                                                    new Tuple<int, int>(2, 0),
                                                    new Tuple<int, int>(2, 1),
                                                    new Tuple<int, int>(2, 2),
                                                };
            var results = counterPeriodCombination.Select(
                i =>
                    new Tuple<Tuple<int, int>, List<TopRatingStatisticsDocument>>(
                        i,
                        this.statisticsFindQuery(i.Item1, i.Item2))).ToDictionary(k => k.Item1, v => v.Item2);

            return new TopRatingStatistics()
            {
                ManualStepsCount = new TopStatisticsAggregate()
                {
                    Day = this.GetTopStatisticsCounters(results[new Tuple<int, int>(2, 2)]),
                    Week = this.GetTopStatisticsCounters(results[new Tuple<int, int>(2, 1)]),
                    Total = this.GetTopStatisticsCounters(results[new Tuple<int, int>(2, 0)]),
                },
                TotalEarned = new TopStatisticsAggregate()
                {
                    Day = this.GetTopStatisticsCounters(results[new Tuple<int, int>(1, 2)]),
                    Week = this.GetTopStatisticsCounters(results[new Tuple<int, int>(1, 1)]),
                    Total = this.GetTopStatisticsCounters(results[new Tuple<int, int>(1, 0)]),
                },
                RootCounter = new TopStatisticsAggregate()
                {
                    Day = this.GetTopStatisticsCounters(results[new Tuple<int, int>(0, 2)]),
                    Week = this.GetTopStatisticsCounters(results[new Tuple<int, int>(0, 1)]),
                    Total = this.GetTopStatisticsCounters(results[new Tuple<int, int>(0, 0)]),
                },
            };
        }

        private TopStatisticsCounter[] GetTopStatisticsCounters(IEnumerable<TopRatingStatisticsDocument> source)
        {
            return source.Select(i => new TopStatisticsCounter() { Login = i.Login, UserId = i.UserId, Value = i.Value })
                .ToArray();
        }

        public void CreateOrUpdateTopRatingStatistics(TopRatingStatistics topRatingStatistics)
        {
            IMongoCollection<TopRatingStatisticsDocument> collection = this.Database.GetCollection<TopRatingStatisticsDocument>("TopRatingStatistics");
            var ratingStatisticsDocuments = this._topRatingStatisticsDocumentsMapper.Map(topRatingStatistics);
            if (ratingStatisticsDocuments.Count > 0)
            {
                collection.InsertMany(ratingStatisticsDocuments);
            }
        }

        public void DropStatistics()
        {
            this.Database.DropCollection("TopRatingStatistics");
        }
    }
}