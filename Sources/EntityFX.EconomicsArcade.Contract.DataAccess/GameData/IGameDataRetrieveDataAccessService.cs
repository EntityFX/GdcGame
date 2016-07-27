using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Common.UserRating;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;

namespace EntityFX.EconomicsArcade.Contract.DataAccess.GameData
{
    [ServiceContract]
    public interface IGameDataRetrieveDataAccessService
    {
        [OperationContract]
        Common.GameData GetGameData(int userId);
        [OperationContract]
        UserRating[] GetUserRatings();
    }
}
