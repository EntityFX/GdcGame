namespace EntityFX.Gdcame.DataAccess.Repository.Contract.RatingServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;

    public interface IGlobalRatingRepository
    {
        TopRatingStatistics GetRaiting(int top = 500);

        void CreateOrUpdateTopRatingStatistics(TopRatingStatistics topRatingStatistics);
    }
}