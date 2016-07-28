using EntityFX.EconomicsArcade.Contract.Common.UserRating;

namespace EntityFX.EconomicsArcade.Presentation.Controllers
{
    public interface IRatingApiController
    {
        UserRating[] GetUsersRatingByCount(int count);

        UserRating FindUserRatingByUserName();

        UserRating[] FindUserRatingByUserNameAndAroundUsers(int count);
    }
}