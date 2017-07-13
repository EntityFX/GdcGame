using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Contract.Controller.Common
{
    public interface IRatingController
    {
        Task<TopRatingStatisticsModel> GetRaiting(int top = 500);
    }
}
