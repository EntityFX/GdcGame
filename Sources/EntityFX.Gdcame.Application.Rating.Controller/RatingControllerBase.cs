
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Application.Rating.Controller
{
    public abstract class RatingControllerBase : ApiController, IRatingController
    {
        private readonly IRatingManager _raitingManager;
        private readonly IMapper<TopRatingStatistics, TopRatingStatisticsModel> _topRatingStatisticsMapper;

        public RatingControllerBase(IRatingManager raitingManager, IMapperFactory mapperFactory)
        {
            _raitingManager = raitingManager;
            _topRatingStatisticsMapper =  mapperFactory.Build<TopRatingStatistics, TopRatingStatisticsModel>();
        }

        public virtual Task<TopRatingStatisticsModel> GetRaiting(int top = 500)
        {
            return Task.Factory.StartNew(() => _topRatingStatisticsMapper.Map(_raitingManager.GetRaiting(top)));
        }
    }


}
