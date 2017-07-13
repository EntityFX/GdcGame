using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;


namespace EntityFX.Gdcame.Application.Api.Common
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
