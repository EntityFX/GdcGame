namespace EntityFX.Gdcame.DataAccess.Contract.Common.Rating
{
    using System.ServiceModel;

    using EntityFX.Gdcame.Common.Contract.UserRating;

    [ServiceContract]
    public interface IRatingDataAccess
    {
        [OperationContract]
        TopRatingStatistics GetRaiting(int top = 500);
    }
}
