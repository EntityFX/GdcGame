using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.Manager.Contract.RatingManager
{
    [ServiceContract]
    public interface IRatingManager
    {
        [OperationContract]
        RatingStatisticsUserInfo[] GetRaiting(int top = 500);
    }
}