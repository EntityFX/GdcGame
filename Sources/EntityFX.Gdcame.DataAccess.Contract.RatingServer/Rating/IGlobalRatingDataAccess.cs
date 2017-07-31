namespace EntityFX.Gdcame.DataAccess.Contract.RatingServer.Rating
{
    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;

    public interface IGlobalRatingDataAccess : IRatingDataAccess
    {
        void PeristTopRatingStatisticsFromNode(TopRatingStatistics topRatingStatistics);
    }
}