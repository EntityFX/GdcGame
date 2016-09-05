using System.Web.Http;
using System.Web.Http.Controllers;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Presentation.Contract.Controller;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;

namespace EntityFX.Gdcame.Presentation.Web.Controller
{
    public class RatingApiController : ApiController, IRatingApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        public RatingApiController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;
        }

        [HttpPost]
        public UserRating[] GetRaiting([FromBody] int count)
        {
            return new UserRating[0];
            //_gameDataProvider.GetUsersRatingByCount(count);
        }

        [HttpGet]
        public UserRating GetUserRating()
        {
            return new UserRating();
            //return _gameDataProvider.FindUserRatingByUserName(User.Identity.Name);
        }


        [HttpPost]
        public UserRating[] GetNearestRating([FromBody] int count)
        {
            return new UserRating[0];
            //return _gameDataProvider.FindUserRatingByUserNameAndAroundUsers(User.Identity.Name, count);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _gameDataProvider.InitializeSession(User.Identity.Name);
        }
    }
}