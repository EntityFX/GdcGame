namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.DataAccess.Repository.Mongo.RatingServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    internal class TopRatingStatisticsDocumentMapper : IMapper<TopRatingStatistics, List<TopRatingStatisticsDocument>>
    {
        public List<TopRatingStatisticsDocument> Map(TopRatingStatistics source, List<TopRatingStatisticsDocument> destination = null)
        {

            var ratingStatisticsDocuments = new List< TopRatingStatisticsDocument > ();
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.RootCounter.Total, 0, 0));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.RootCounter.Week, 0, 1));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.RootCounter.Day, 0, 2));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.TotalEarned.Total, 1, 0));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.TotalEarned.Week, 1, 1));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.TotalEarned.Day, 1, 2));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.ManualStepsCount.Total, 2, 0));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.ManualStepsCount.Week, 2, 1));
            ratingStatisticsDocuments.AddRange(MapForTopCounter(source.ManualStepsCount.Day, 2, 2));
            return ratingStatisticsDocuments;
        }

        private static IEnumerable<TopRatingStatisticsDocument> MapForTopCounter(TopStatisticsCounter[] topStatisticsCounter,
            int counterType, int periodType)
        {
            return topStatisticsCounter.Select(_ => new TopRatingStatisticsDocument()
            {
                UserId = _.UserId,
                Login = _.Login,
                CounterType = 0,
                PeriodType = 1,
                Value = _.Value,
                CreateDateTime = DateTime.Now
            });
        }
    }
}