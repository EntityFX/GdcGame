using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.DataAccess.Contract.GameData;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserGameSnapshotRepository
    {
        StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion);
        void CreateForUser(int userId, StoredGameData gameData);
        void UpdateForUser(int userId, StoredGameData gameData); 
    }
}