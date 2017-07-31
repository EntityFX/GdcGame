namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot;

    public interface IUserGameSnapshotRepository
    {
        StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion);
        void CreateOrUpdateUserGames(StoredGameDataWithUserId[] gameDataWithUserId);
    }
}