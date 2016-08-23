using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.Presentation.Web.Controller
{
    public interface IRatingApiController
    {
        UserRating[] GetRaiting(int count);

        UserRating GetUserRating();

        UserRating[] GetNearestRating(int count);
    }
}