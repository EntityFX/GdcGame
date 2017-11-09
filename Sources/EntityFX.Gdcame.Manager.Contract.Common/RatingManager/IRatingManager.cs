//using System.ServiceModel;

namespace EntityFX.Gdcame.Manager.Contract.Common.RatingManager
{
    using EntityFX.Gdcame.Contract.Common.UserRating;

    //[ServiceContract]
    public interface IRatingManager
    {
        //[OperationContract]
        TopRatingStatistics GetRaiting(int top = 500);
    }
}