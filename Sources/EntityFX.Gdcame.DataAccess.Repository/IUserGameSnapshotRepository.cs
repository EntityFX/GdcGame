using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.DataAccess.Contract.GameData;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserGameSnapshotRepository
    {
        StoreGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion);
        void CreateForUser(int userId, StoreGameData gameData);
        void UpdateForUser(int userId, StoreGameData gameData); 
    }
}