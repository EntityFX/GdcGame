namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System.Threading.Tasks;

    public interface IRatingDataRetrieve
    {
        Task<TopRatingStatistics> GetRaiting(int top = 500);
    }
}