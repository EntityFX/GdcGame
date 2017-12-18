using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    using EntityFX.Gdcame.Application.Api.Common;
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "System")]
    [Route("api/rating")]
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
