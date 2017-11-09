namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.Rating
{
    using System;
    //using System.ServiceModel;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;

    //[ServiceContract]
    public interface ILocalNodeRatingDataAccess : IRatingDataAccess
    {
        //[OperationContract]
        void PersistUsersRatingHistory(RatingHistory[] ratingHistory);

        //[OperationContract]
        RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period);

        //[OperationContract]
        void CleanOldHistory(TimeSpan period);

        //[OperationContract]
        void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);
    }
}
