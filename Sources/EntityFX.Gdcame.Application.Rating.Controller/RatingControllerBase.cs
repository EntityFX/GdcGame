
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EntityFX.Gdcame.Application.Rating.Controller
{
    public abstract class RatingControllerBase : ApiController, IRatingController
    {
        private readonly IRatingManager _raitingManager;
        public RatingControllerBase(IRatingManager raitingManager)
        {
            _raitingManager = raitingManager;
        }

        public virtual RatingStatisticsModel[] GetRaiting(int top = 500)
        {
            return ConvertAllRaitingStatisticsAsRaitingStatisticsModel(_raitingManager.GetRaiting(top));
        }

        private RatingStatisticsModel[] ConvertAllRaitingStatisticsAsRaitingStatisticsModel(RatingStatistics[] raitingStatistics)
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<RatingStatistics, RatingStatisticsModel>();
            //    cfg.CreateMap<CountValues, CountValuesModel>();
            //});
            List<RatingStatisticsModel> AllRSModel = new List<RatingStatisticsModel>();
            for (int i = 0; i < raitingStatistics.Length; i++)
            {
                AllRSModel.Add(ConvertRaitingStatisticsAsRaitingStatisticsModel(raitingStatistics[i]));
                //RatingStatisticsModel _nextRSModel = Mapper.Map<RatingStatistics, RatingStatisticsModel>(raitingStatistics[i]);
               // AllRSModel.Add(_nextRSModel);
            }
            //foreach (var _raiting in raitingStatistics)
            //{
            //    RatingStatisticsModel _nextRSModel = Mapper.Map<RatingStatistics, RatingStatisticsModel>(_raiting);
            //}
            return AllRSModel.ToArray();
        }
        private RatingStatisticsModel ConvertRaitingStatisticsAsRaitingStatisticsModel(RatingStatistics RatingStatistics)
        {

            RatingStatisticsModel _RSModel = new RatingStatisticsModel
            {
                UserID = RatingStatistics.UserId,
                MunualStepsCount = ConvertCountValuesModelAsCountValues(RatingStatistics.MunualStepsCount),
                RootCounter = ConvertCountValuesModelAsCountValues(RatingStatistics.RootCounter),
                TotalEarned = ConvertCountValuesModelAsCountValues(RatingStatistics.TotalEarned)

            };

            return _RSModel;
        }
        private CountValuesModel ConvertCountValuesModelAsCountValues(CountValues _countValues)
        {
            CountValuesModel CVModel = new CountValuesModel
            {
                Day = _countValues.Day,
                Week = _countValues.Week,
                Total = _countValues.Total,
            };
            return CVModel;
        }
    }

   
}
