using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.Presentation.Web.Controller
{
    public interface IRatingApiController
    {
        UserRating[] GetUsersRatingByCount(int count);

        UserRating FindUserRatingByUserName();

        UserRating[] FindUserRatingByUserNameAndAroundUsers(int count);
    }
}