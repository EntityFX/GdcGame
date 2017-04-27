using System.Linq;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Application.WebApi.Mappers
{
    public class TopRatingStatisticsModelMapper : IMapper<TopRatingStatistics, TopRatingStatisticsModel>
    {
        public TopRatingStatisticsModel Map(TopRatingStatistics source, TopRatingStatisticsModel destination = null)
        {
            return new TopRatingStatisticsModel()
            {
                ManualStepsCount = MapRatingStatisticsAggregate(source.ManualStepsCount),
                TotalEarned = MapRatingStatisticsAggregate(source.TotalEarned),
                RootCounter = MapRatingStatisticsAggregate(source.RootCounter),
            };
        }

        private static TopStatisticsAggregateModel MapRatingStatisticsAggregate(TopStatisticsAggregate source)
        {
            return new TopStatisticsAggregateModel()
            {
                Day = source.Day.Select(MapRatingStatisticsCounter).ToArray(),
                Week = source.Week.Select(MapRatingStatisticsCounter).ToArray(),
                Total = source.Total.Select(MapRatingStatisticsCounter).ToArray(),
            };
        }

        private static TopStatisticsCounterModel MapRatingStatisticsCounter(TopStatisticsCounter source)
        {
            return new TopStatisticsCounterModel()
            {
                UserId = source.UserId,
                Login = source.Login,
                Value = source.Value,
            };
        }
    }
}