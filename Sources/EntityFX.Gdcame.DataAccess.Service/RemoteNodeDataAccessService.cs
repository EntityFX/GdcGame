namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;

    public class RemoteNodeDataAccessService : IRatingDataAccess
    {
        public TopRatingStatistics GetRaiting(int top = 500)
        {
            throw new System.NotImplementedException();
        }
    }
}