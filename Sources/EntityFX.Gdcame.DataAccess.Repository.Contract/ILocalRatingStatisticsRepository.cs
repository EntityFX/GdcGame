namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;

    public interface ILocalRatingStatisticsRepository
    {
        void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);
        TopRatingStatistics GetRaiting(int top = 500);
    }
}
