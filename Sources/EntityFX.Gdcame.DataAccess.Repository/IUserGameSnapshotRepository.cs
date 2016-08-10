using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserGameSnapshotRepository
    {
        GameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion);
        void CreateForUser(int userId, GameData gameData);
        void UpdateForUser(int userId, GameData gameData); 
    }
}