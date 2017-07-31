namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using System;

    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.RatingHistory;

    public interface IRatingHistoryRepository
    {
        void PersistUsersRatingHistory(RatingHistory[] usersRatingHistory);
        RatingHistory[] ReadHistoryWithUsersIds(GetUsersRatingHistoryCriterion usersRatingHistoryCriterion);
        void CleanOldHistory(TimeSpan period);
    }
}
