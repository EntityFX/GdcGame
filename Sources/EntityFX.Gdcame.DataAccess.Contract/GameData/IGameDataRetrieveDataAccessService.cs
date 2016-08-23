using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [ServiceContract]
    public interface IGameDataRetrieveDataAccessService
    {
        [OperationContract]
        Common.Contract.GameData GetGameData(string userId);

        [OperationContract]
        UserRating[] GetUserRatings();
    }
}