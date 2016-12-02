using EntityFX.Gdcame.Application.Rating.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Application.WebApi.Controller
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
