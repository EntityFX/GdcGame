using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.Infrastructure.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class GlobalRatingRepository : IGlobalRatingRepository
    {
        private readonly IMapper<TopRatingStatistics, List<TopRatingStatisticsDocument>> _topRatingStatisticsMapper;

        private IMongoDatabase Database
        {
            get; set;
        }
        public GlobalRatingRepository(IMongoDatabase database, IMapperFactory mapperFactory)
        {
            Database = database;
            _topRatingStatisticsMapper = mapperFactory.Build<TopRatingStatistics, List<TopRatingStatisticsDocument>>();
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
            throw new System.NotImplementedException();
        }

        public void CreateOrUpdateTopRatingStatistics(TopRatingStatistics topRatingStatistics)
        {
            IMongoCollection<TopRatingStatisticsDocument> collection = Database.GetCollection<TopRatingStatisticsDocument>("TopRatingStatistics");
            collection.InsertMany(_topRatingStatisticsMapper.Map(topRatingStatistics));
        }
    }

    class TopRatingStatisticsDocument
    {
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public string Login { get; set; }
        public decimal Value { get; set; }
        public int CounterType { get; set; }
        public int PeriodType { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}