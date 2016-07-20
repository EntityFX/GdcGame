using System.Web.Http;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public interface IRatingApiController
    {
        UserRating[] GetUsersRatingByCount(int count);

        UserRating FindUserRatingByUserName();

        UserRating[] FindUserRatingByUserNameAndAroundUsers(int count);
    }
}