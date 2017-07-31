namespace EntityFX.Gdcame.DataAccess.Service.RatingServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.RatingServer.Rating;

    public class GlobalRatingDataAccess : IGlobalRatingDataAccess
    {
        private readonly IGlobalRatingDataAccess _globalRatingDataAccess;

        public GlobalRatingDataAccess(IGlobalRatingDataAccess globalRatingDataAccess)
        {
            this._globalRatingDataAccess = globalRatingDataAccess;
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
            return this._globalRatingDataAccess.GetRaiting(top);
        }

        public void PeristTopRatingStatisticsFromNode(TopRatingStatistics topRatingStatistics)
        {
            this._globalRatingDataAccess.PeristTopRatingStatisticsFromNode(topRatingStatistics);
        }
    }
}