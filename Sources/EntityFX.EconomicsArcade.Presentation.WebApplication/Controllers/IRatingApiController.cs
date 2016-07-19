using System.Web.Http;
using EntityFX.EconomicsArcade.Contract.Common;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public interface IRatingApiController
    {
        UserRating[] GetUsersRatingByCount(int count);

        UserRating FindUserRatingByUserName();

        UserRating[] FindUserRatingByUserNameAndAroundUsers(int count);
    }
}