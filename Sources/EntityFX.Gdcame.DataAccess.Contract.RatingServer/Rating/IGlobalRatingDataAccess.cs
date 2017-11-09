namespace EntityFX.Gdcame.DataAccess.Contract.RatingServer.Rating
{
    //using System.ServiceModel;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;

    //[ServiceContract]
    public interface IGlobalRatingDataAccess : IRatingDataAccess
    {
        //[OperationContract]
        void CreateOrUpdateUsersRatingStatistics(TopRatingStatistics ratingStatistics);
        //[OperationContract]
        void DropStatistics();
    }
}
