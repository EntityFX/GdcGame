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

namespace EntityFX.Gdcame.Application.Rating
{
    [RoutePrefix("api/rating")]
    [AllowAnonymous()]
    public class RatingController : RatingControllerBase, IRatingController
    {
        public RatingController(IRatingManager raitingManager) : base(raitingManager)
        {
        }

        [HttpGet]
        [Route("")]
        public override Task<RatingStatisticsModel[]> GetRaiting(int top = 500)
        {
            return base.GetRaiting(top);
        }
    }
}
