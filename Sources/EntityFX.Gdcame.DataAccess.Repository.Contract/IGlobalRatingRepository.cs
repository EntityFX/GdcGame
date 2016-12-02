using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IGlobalRatingRepository
    {
        TopRatingStatistics GetRaiting(int top = 500);

        void CreateOrUpdateTopRatingStatistics(TopRatingStatistics topRatingStatistics);
    }
}