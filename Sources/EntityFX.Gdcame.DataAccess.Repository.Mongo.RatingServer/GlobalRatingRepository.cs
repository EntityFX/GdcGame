namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer
{
    using System.Collections.Generic;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.RatingServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    using MongoDB.Driver;

    public class GlobalRatingRepository : IGlobalRatingRepository
    {
        private readonly IMapper<TopRatingStatistics, List<TopRatingStatisticsDocument>> _topRatingStatisticsMapper;

        private IMongoDatabase Database
        {
            get; set;
        }
        public GlobalRatingRepository(IMongoDatabase database, IMapperFactory mapperFactory)
        {
            this.Database = database;
            this._topRatingStatisticsMapper = mapperFactory.Build<TopRatingStatistics, List<TopRatingStatisticsDocument>>();
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
            throw new System.NotImplementedException();
        }

        public void CreateOrUpdateTopRatingStatistics(TopRatingStatistics topRatingStatistics)
        {
            IMongoCollection<TopRatingStatisticsDocument> collection = this.Database.GetCollection<TopRatingStatisticsDocument>("TopRatingStatistics");
            collection.InsertMany(this._topRatingStatisticsMapper.Map(topRatingStatistics));
        }
    }
}