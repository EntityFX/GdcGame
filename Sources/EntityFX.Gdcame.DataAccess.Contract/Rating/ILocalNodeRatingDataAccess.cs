namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.Rating
{
    using System;
    //

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;

    //
    public interface ILocalNodeRatingDataAccess : IRatingDataAccess
    {
        //
        void PersistUsersRatingHistory(RatingHistory[] ratingHistory);

        //
        RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period);

        //
        void CleanOldHistory(TimeSpan period);

        //
        void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);
    }
}
