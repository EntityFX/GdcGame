using EntityFX.Gdcame.Common.Contract.UserRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Contract.Rating
{
    [ServiceContract]
    public interface IRatingDataAccessBase
    {
        [OperationContract]
        void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);

        [OperationContract]
        RatingStatistics[] GetRaiting(int top = 500);
    }
}
