namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer
{
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot;

    using MongoDB.Driver;
    using System.Collections.Generic;
    using System;
    using MongoDB.Bson;

    public class UserGameSnapshotRepository : IUserGameSnapshotRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }

        public UserGameSnapshotRepository(IMongoDatabase database)
        {
            this.Database = database;
        }

        public StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
        {
            IMongoCollection<StoredGameDataWithUserId> storedGameDataWithUserId = this.Database.GetCollection<StoredGameDataWithUserId>("StoredGameDataWithUserId");
            var filter = Builders<StoredGameDataWithUserId>.Filter.Eq("UserId", criterion.UserId);
            var result = storedGameDataWithUserId.Find(filter).ToList().FirstOrDefault();
            return result  == null ? null : result.StoredGameData;
        }

        public StoredGameDataWithUserId[] FindChunked(GetGameSnapshotsByOffsetCriterion offsetCriterion)
        {
            IMongoCollection<StoredGameDataWithUserId> storedGamesData = this.Database.GetCollection<StoredGameDataWithUserId>("StoredGameDataWithUserId");
            var filter = Builders<StoredGameDataWithUserId>.Filter
                .Nin(x => x.UserId, offsetCriterion.UserIds.Select(l => l));
            var result = storedGamesData.Find(filter).Skip(offsetCriterion.Offset).Limit(offsetCriterion.Size).ToList().ToArray();
            return result == null ? null : result;
        }

        public void CreateUserGames(StoredGameDataWithUserId[] gameDataWithUserId)
        {
            IMongoCollection<StoredGameDataWithUserId> users = this.Database.GetCollection<StoredGameDataWithUserId>("StoredGameDataWithUserId");
            users.InsertMany(gameDataWithUserId);
        }
    
        public void CreateOrUpdateUserGames(StoredGameDataWithUserId[] gameDataWithUserId)
        {
            IMongoCollection<StoredGameDataWithUserId> users = this.Database.GetCollection<StoredGameDataWithUserId>("StoredGameDataWithUserId");

            var groupedByUserId = gameDataWithUserId.GroupBy(_ => _.UserId).Select(_ => _.First()).ToArray();
            var usersFilter = Builders<StoredGameDataWithUserId>.Filter.In("UserId", groupedByUserId.Select(_ => _.UserId));
            var usersIdsForUpdateList = users.Find(usersFilter).Project(_ => _.UserId).ToList();


            var gameDataForUpdate = groupedByUserId.Where(_ => usersIdsForUpdateList.Contains(_.UserId));
            var gameDataToCreate = groupedByUserId.Where(_ => !usersIdsForUpdateList.Contains(_.UserId));

            if (gameDataToCreate.Any())
            {
                users.InsertMany(gameDataToCreate);
            }

            var models = gameDataForUpdate.Select(update => new ReplaceOneModel<StoredGameDataWithUserId>(Builders<StoredGameDataWithUserId>.Filter.Eq(s => s.UserId, update.UserId), update) { IsUpsert = true}).ToList();
            if (models.Count > 0)
            {
                users.BulkWrite(models);
            }
        }
    }
}
