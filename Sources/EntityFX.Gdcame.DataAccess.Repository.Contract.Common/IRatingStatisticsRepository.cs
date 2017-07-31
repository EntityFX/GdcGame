namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common
{
    using EntityFX.Gdcame.Common.Contract.UserRating;

    public interface IRatingStatisticsRepository
    {
        void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);
        TopRatingStatistics GetRaiting(int top = 500);
    }
}
