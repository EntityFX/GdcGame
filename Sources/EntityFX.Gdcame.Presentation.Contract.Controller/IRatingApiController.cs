using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.Application.Contract.Controller
{
    public interface IRatingApiController
    {
        UserRating[] GetRaiting(int count);

        UserRating GetUserRating();

        UserRating[] GetNearestRating(int count);
    }
}