using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.Manager.Contract.RatingManager
{
    [ServiceContract]
    public interface IRatingManager
    {
        [OperationContract]
        TopRatingStatistics GetRaiting(int top = 500);
    }
}