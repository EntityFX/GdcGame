using System.Web.Http;
using System.Web.Http.Controllers;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;

namespace EntityFX.Gdcame.Presentation.Web.Controller
{
    public class RatingApiController : ApiController, IRatingApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _gameDataProvider.InitializeSession(User.Identity.Name);
        }

        public RatingApiController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;
        }

        [HttpPost]
        public UserRating[] GetUsersRatingByCount([FromBody]int count)
        {
            return _gameDataProvider.GetUsersRatingByCount(count);
        }

        [HttpGet]
        public UserRating FindUserRatingByUserName()
        {
            return _gameDataProvider.FindUserRatingByUserName(User.Identity.Name);
        }


        [HttpPost]
        public UserRating[] FindUserRatingByUserNameAndAroundUsers([FromBody]int count)
        {
            return _gameDataProvider.FindUserRatingByUserNameAndAroundUsers(User.Identity.Name, count);
        }
    }

}
