﻿using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class UserGameSnapshotRepository : IUserGameSnapshotRepository
    {
        public StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
        {
            return null;
        }

        public void CreateUserGames(StoredGameDataWithUserId[] gameDataWithUserId)
        {
        }
    
        public void UpdateUserGames(StoredGameDataWithUserId[] gameDataWithUserId)
        {
        }
    }
}
