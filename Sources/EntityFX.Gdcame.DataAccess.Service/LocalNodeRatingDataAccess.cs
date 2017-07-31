namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using System;

    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.Rating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.RatingHistory;

    class LocalNodeRatingDataAccess : ILocalNodeRatingDataAccess
    {
        private readonly IRatingStatisticsRepository _ratingStatisticsRepository;
        private readonly IRatingHistoryRepository _ratingHistoryRepository;

        public LocalNodeRatingDataAccess(IRatingStatisticsRepository ratingStatisticsRepository, IRatingHistoryRepository ratingHistoryRepository)
        {
            this._ratingStatisticsRepository = ratingStatisticsRepository;
            this._ratingHistoryRepository = ratingHistoryRepository;
        }

        public void PersistUsersRatingHistory(RatingHistory[] ratingHistory)
        {
            this._ratingHistoryRepository.PersistUsersRatingHistory(ratingHistory);
        }

        public RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period)
        {
            return this._ratingHistoryRepository.ReadHistoryWithUsersIds(new GetUsersRatingHistoryCriterion(){ UsersIds =  userslds, Period = period});
        }

        public void CleanOldHistory(TimeSpan period)
        {
            this._ratingHistoryRepository.CleanOldHistory(period);
        }

        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            this._ratingStatisticsRepository.CreateOrUpdateUsersRatingStatistics(ratingStatistics);
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
           return this._ratingStatisticsRepository.GetRaiting(top);
        }

    }

}

