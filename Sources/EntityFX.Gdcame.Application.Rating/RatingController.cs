using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Rating.Controller;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Application.Rating
{
    [RoutePrefix("api/rating")]
    [AllowAnonymous()]
    public class RatingController : RatingControllerBase, IRatingController
    {
        public RatingController(IRatingManager raitingManager, IMapperFactory mapperFactory) : base(raitingManager, mapperFactory)
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
