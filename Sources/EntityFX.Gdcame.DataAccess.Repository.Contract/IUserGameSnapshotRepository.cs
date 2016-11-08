using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IUserGameSnapshotRepository
    {
        StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion);
        void CreateOrUpdateUserGames(StoredGameDataWithUserId[] gameDataWithUserId);
    }
}