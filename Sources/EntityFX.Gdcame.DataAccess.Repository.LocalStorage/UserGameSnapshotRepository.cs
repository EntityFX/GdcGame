using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;
using System.IO;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage
{
    public class UserGameSnapshotRepository : IUserGameSnapshotRepository
    {
        public UserGameSnapshotRepository()
        {
            if (!Directory.Exists(Path.Combine("storage", "game-snapshots")))
            {
                Directory.CreateDirectory(Path.Combine("storage", "game-snapshots"));
            }
        }

        public StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
        {
            if (!File.Exists(Path.Combine("storage", "game-snapshots", criterion.UserId + ".json")))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(Path.Combine("storage", "game-snapshots", criterion.UserId + ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (StoredGameData)serializer.Deserialize(file, typeof(StoredGameData));
            }
        }

        public void CreateUserGames(StoredGameDataWithUserId[] listOfGameDataWithUserId)
        {
            foreach (var gameDataWithUserId in listOfGameDataWithUserId)
            {
                using (StreamWriter file = File.CreateText(Path.Combine("storage", "game-snapshots", gameDataWithUserId.UserId + ".json")))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.Auto;
                    serializer.Serialize(file, gameDataWithUserId.StoredGameData);
                }
            }
        }

        public void CreateOrUpdateUserGames(StoredGameDataWithUserId[] listOfGameDataWithUserId)
        {
            CreateUserGames(listOfGameDataWithUserId);
        }
    }
}
