namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData
{
    using System.ServiceModel;

    using EntityFX.Gdcame.Common.Contract;
    using EntityFX.Gdcame.Common.Contract.UserRating;

    [ServiceContract]
    public interface IGameDataRetrieveDataAccessService
    {
        [OperationContract]
        GameData GetGameData(string userId);

        [OperationContract]
        RatingStatistics[] GetUserRatings();
    }
}