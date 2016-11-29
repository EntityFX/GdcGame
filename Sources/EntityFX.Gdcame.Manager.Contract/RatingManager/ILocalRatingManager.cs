using EntityFX.Gdcame.Common.Contract.UserRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Manager.Contract.RatingManager
{
    [ServiceContract]
    public interface ILocalRatingManager
    {
        [OperationContract]
        RatingStatistics[] GetRaiting(int top = 500);
    }
}
