using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class RemoteNodeDataAccessService : IRatingDataAccess
    {
        public TopRatingStatistics GetRaiting(int top = 500)
        {
            throw new System.NotImplementedException();
        }
    }
}