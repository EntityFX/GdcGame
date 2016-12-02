
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

        public virtual Task<RatingStatisticsModel[]> GetRaiting(int top = 500)
        {
            return Task.Factory.StartNew(() => ConvertAllRaitingStatisticsAsRaitingStatisticsModel(_raitingManager.GetRaiting(top)));
        }

        private RatingStatisticsModel[] ConvertAllRaitingStatisticsAsRaitingStatisticsModel(RatingStatisticsUserInfo[] raitingStatistics)
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<RatingStatistics, RatingStatisticsModel>();
            //    cfg.CreateMap<CountValues, CountValuesModel>();
            //});
            List<RatingStatisticsModel> allRsModel = new List<RatingStatisticsModel>();
            for (int i = 0; i < raitingStatistics.Length; i++)
            {
                allRsModel.Add(ConvertRaitingStatisticsAsRaitingStatisticsModel(raitingStatistics[i]));
                //RatingStatisticsModel _nextRSModel = Mapper.Map<RatingStatistics, RatingStatisticsModel>(raitingStatistics[i]);
                // AllRSModel.Add(_nextRSModel);
            }
            //foreach (var _raiting in raitingStatistics)
            //{
            //    RatingStatisticsModel _nextRSModel = Mapper.Map<RatingStatistics, RatingStatisticsModel>(_raiting);
            //}
            return allRsModel.ToArray();
        }
        private RatingStatisticsModel ConvertRaitingStatisticsAsRaitingStatisticsModel(RatingStatisticsUserInfo ratingStatistics)
        {

            RatingStatisticsModel rsModel = new RatingStatisticsModel
            {
                Login = ratingStatistics.Login,
                UserID = ratingStatistics.UserId,
                MunualStepsCount = ConvertCountValuesModelAsCountValues(ratingStatistics.ManualStepsCount),
                RootCounter = ConvertCountValuesModelAsCountValues(ratingStatistics.RootCounter),
                TotalEarned = ConvertCountValuesModelAsCountValues(ratingStatistics.TotalEarned)

            };

            return rsModel;
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
