using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.DataAccess.Contract.Rating
{
    public interface IGlobalRatingDataAccess : IRatingDataAccess
    {
        void PeristTopRatingStatisticsFromNode(TopRatingStatistics topRatingStatistics);
    }
}