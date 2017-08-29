namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot;
    using EntityFX.Gdcame.Contract.MainServer.Store;

    public interface IUserGameSnapshotRepository
    {
        StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion);
        void CreateOrUpdateUserGames(StoredGameDataWithUserId[] gameDataWithUserId);
        StoredGameDataWithUserId[] FindChunked(GetGameSnapshotsByOffsetCriterion criterion);
    }
}