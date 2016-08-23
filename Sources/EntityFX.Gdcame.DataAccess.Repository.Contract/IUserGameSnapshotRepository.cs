using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IUserGameSnapshotRepository
    {
        StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion);
        void CreateForUser(string userId, StoredGameData gameData);
        void UpdateForUser(string userId, StoredGameData gameData);
    }
}