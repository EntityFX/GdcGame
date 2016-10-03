using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;
using MongoDB.Driver;
using EntityFX.Gdcame.DataAccess.Contract.User;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class UserGameSnapshotRepository : IUserGameSnapshotRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }

        public UserGameSnapshotRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
        {
            IMongoCollection<StoredGameDataWithUserId> storedGameDataWithUserId = Database.GetCollection<StoredGameDataWithUserId>("StoredGameDataWithUserId");
            var filter = Builders<StoredGameDataWithUserId>.Filter.Eq("UserId", criterion.UserId);
            var result = storedGameDataWithUserId.Find(filter).ToList().FirstOrDefault();
            return result  == null ? null : result.StoredGameData;
        }

        public void CreateUserGames(StoredGameDataWithUserId[] gameDataWithUserId)
        {
            IMongoCollection<StoredGameDataWithUserId> users = Database.GetCollection<StoredGameDataWithUserId>("StoredGameDataWithUserId");
            users.InsertMany(gameDataWithUserId);
        }
    
        public void UpdateUserGames(StoredGameDataWithUserId[] gameDataWithUserId)
        {
            IMongoCollection<StoredGameDataWithUserId> users = Database.GetCollection<StoredGameDataWithUserId>("StoredGameDataWithUserId");
            var filter = Builders<StoredGameDataWithUserId>.Filter.In("UserId", gameDataWithUserId.Select(x => x.UserId));
            
                gameDataWithUserId.ToList().ForEach(
                        x =>
                        {
                            filter = Builders<StoredGameDataWithUserId>.Filter.Eq("UserId", x.UserId);
                            var result = users.Find(filter);

                            if (result.FirstOrDefault() == null)
                                users.InsertOne(x);
                            else
                                result.First().StoredGameData = x.StoredGameData;
                        }
                    );
        }
    }
}
