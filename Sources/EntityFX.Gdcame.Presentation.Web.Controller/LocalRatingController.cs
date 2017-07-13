using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    [Authorize(Roles = "System")]
    [RoutePrefix("api/rating")]
    //[AllowAnonymous()]
    public class LocalRatingController : RatingControllerBase, IRatingController
    {
        public LocalRatingController(IRatingManager raitingManager, IMapperFactory mapperFactory) 
            : base(raitingManager, mapperFactory)
        {
        }

        [HttpGet]
        [Route("")]
        public override Task<TopRatingStatisticsModel> GetRaiting(int top = 500)
        {
            return base.GetRaiting(top);
        }
    }
}
