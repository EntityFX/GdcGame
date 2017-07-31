namespace EntityFX.Gdcame.DataAccess.Service.Common
{
    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;

    public class RatingDataAccess : IRatingDataAccess
    {
        private readonly IRatingStatisticsRepository _ratingStatisticsRepository;

        public RatingDataAccess(IRatingStatisticsRepository ratingRepository)
        {
            this._ratingStatisticsRepository = ratingRepository;
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
