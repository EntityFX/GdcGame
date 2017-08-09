using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Controller.Common;


namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    using EntityFX.Gdcame.Application.Api.Common;
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;

    [Authorize(Roles = "System")]
    [RoutePrefix("api/rating")]
    public class LocalRatingController : RatingControllerBase, IRatingController
    {


        private readonly IRatingManager _raitingManager;

        public LocalRatingController(IRatingManager raitingManager, IMapperFactory mapperFactory)
            :base(raitingManager, mapperFactory)
        {
        }

        [HttpGet]
        [Route("")]
        public override Task<TopRatingStatistics> GetRaiting(int top = 500)
        {
            return base.GetRaiting(top);
        }

    }
}
