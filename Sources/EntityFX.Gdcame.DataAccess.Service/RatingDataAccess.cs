using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Contract;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class RatingDataAccess : IRatingDataAccess
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingDataAccess(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }
        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            throw new NotImplementedException();
        }

        public RatingStatistics[] GetRaiting(int top = 500)
        {

            return _ratingRepository.GetRaiting(top);
            //RatingStatistics[] rating =
            //  {
            //    new RatingStatistics
            //    {
            //        UserID = "Yaroslav",
            //        MunualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
            //        TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
            //         RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            //    },
            //    new RatingStatistics
            //    {
            //        UserID = "70",
            //        MunualStepsCount = new CountValues { Day = 548, Week = 658, Total = 1860 },
            //        TotalEarned = new CountValues { Day = 564, Week = 688, Total = 1234 },
            //        RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            //    }
            //};
            //return rating;
        }
    }
}
