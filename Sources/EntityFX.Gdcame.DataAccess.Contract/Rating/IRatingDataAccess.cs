using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.DataAccess.Contract.Rating
{
    [ServiceContract]
    public interface IRatingDataAccess
    {
        [OperationContract]
        void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);

        [OperationContract]
        RatingStatisticsUserInfo[] GetRaiting(int top = 500);
    }
}
