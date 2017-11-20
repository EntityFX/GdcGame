using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;
using Microsoft.AspNetCore.Mvc;


namespace EntityFX.Gdcame.Application.Api.Common
{
    using EntityFX.Gdcame.Contract.Common.UserRating;

    public abstract class RatingControllerBase : Controller, IRatingController
    {
        private readonly IRatingManager _raitingManager;

        public RatingControllerBase(IRatingManager raitingManager, IMapperFactory mapperFactory)
        {
            _raitingManager = raitingManager;
        }

        public virtual Task<TopRatingStatistics> GetRaiting(int top = 500)
        {
            return Task.Factory.StartNew(() => (_raitingManager.GetRaiting(top)));
        }
    }
}
