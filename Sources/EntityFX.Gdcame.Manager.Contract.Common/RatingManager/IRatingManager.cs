//

namespace EntityFX.Gdcame.Manager.Contract.Common.RatingManager
{
    using EntityFX.Gdcame.Contract.Common.UserRating;

    //
    public interface IRatingManager
    {
        //
        TopRatingStatistics GetRaiting(int top = 500);
    }
}