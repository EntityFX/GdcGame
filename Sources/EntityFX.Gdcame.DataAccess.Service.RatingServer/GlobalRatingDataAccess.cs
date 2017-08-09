namespace EntityFX.Gdcame.DataAccess.Service.RatingServer
{
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;
    using EntityFX.Gdcame.DataAccess.Contract.RatingServer.Rating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.RatingServer;

    using Repository.Contract.Common;

    public class GlobalRatingDataAccess : IGlobalRatingDataAccess
    {
        private readonly IGlobalRatingRepository globalRatingRepository;



        public GlobalRatingDataAccess(IGlobalRatingRepository globalRatingRepository)
        {
            this.globalRatingRepository = globalRatingRepository;
        }

        public async Task<TopRatingStatistics> GetRaiting(int top = 500)
        {

            return this.globalRatingRepository.GetRaiting(top);
        }

        public void CreateOrUpdateUsersRatingStatistics(TopRatingStatistics topRatingStatistics)
        {
            this.globalRatingRepository.CreateOrUpdateTopRatingStatistics(topRatingStatistics);
        }

        public void DropStatistics()
        {
            this.globalRatingRepository.DropStatistics();
        }
    }
}