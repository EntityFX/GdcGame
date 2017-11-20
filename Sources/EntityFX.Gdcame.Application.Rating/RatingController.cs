using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Application.Api.Controller.RatingServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;

    [Route("api/rating")]
    [AllowAnonymous()]
    public class RatingController : RatingControllerBase, IRatingController
    {
        public RatingController(IRatingManager raitingManager, IMapperFactory mapperFactory) : base(raitingManager, mapperFactory)
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
