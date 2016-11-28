﻿using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{

    public class RatingStatisticsRepository : IRatingStatisticsRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }
        public RatingStatisticsRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public RatingStatistics[] GetRaiting(int top = 500)
        {
            IMongoCollection<RatingStatisticsBson> collection = Database.GetCollection<RatingStatisticsBson>("RatingStatistics");
            //var filter1 = Builders<RatingStatistics>.Filter.Exists("UserId");
            //var filter2 = Builders<RatingStatistics>.Filter.Exists("MunualStepsCount");
            //var filter3 = Builders<RatingStatistics>.Filter.Exists("TotalEarned");
            //var filter4 = Builders<RatingStatistics>.Filter.Exists("RootCounter");
            //var filterAnd = Builders<RatingStatistics>.Filter.And(new List<FilterDefinition<RatingStatistics>> { filter1, filter2, filter3, filter4 });
            var filter = new BsonDocument();
            var allUser = collection.Find(filter).ToList();
            var uaser500 = ConverterBsonDocumentAsRatingStatistic(allUser).OrderByDescending(a => a.TotalEarned.Total).Take(500).ToArray();

            return uaser500.ToArray();
        }

        private List<RatingStatistics> ConverterBsonDocumentAsRatingStatistic(List<RatingStatisticsBson> bsonRating)
        {
            List<RatingStatistics> rating = new List<RatingStatistics>();
            foreach (var bson in bsonRating)
            {
                rating.Add(new RatingStatistics
                {
                    UserId = bson.UserId,
                    MunualStepsCount = bson.MunualStepsCount,
                    RootCounter=bson.RootCounter,
                    TotalEarned=bson.TotalEarned,
                });
            }
            return rating;
        }


        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            //foreach (var userRating in ratingStatistics)
            //{
            //    CreateOrReplaceUsersRatingStatistics(userRating);
            //}

            IMongoCollection<RatingStatistics> collection = Database.GetCollection<RatingStatistics>("RatingStatistics");

            var groupedByUserId = ratingStatistics.GroupBy(_ => _.UserId).Select(_ => _.First()).ToArray();
            var usersFilter = Builders<RatingStatistics>.Filter.In("UserId", groupedByUserId.Select(_ => _.UserId));
            var usersIdsForUpdateList = collection.Find(usersFilter).Project(_ => _.UserId).ToList();


            var gameDataForUpdate = groupedByUserId.Where(_ => usersIdsForUpdateList.Contains(_.UserId));
            var gameDataToCreate = groupedByUserId.Where(_ => !usersIdsForUpdateList.Contains(_.UserId));

            if (gameDataToCreate.Any())
            {
                collection.InsertMany(gameDataToCreate);
            }

            var models = gameDataForUpdate.Select(update => new ReplaceOneModel<RatingStatistics>(Builders<RatingStatistics>.Filter.Eq(s => s.UserId, update.UserId), update) { IsUpsert = true }).ToList();
            if (models.Count > 0)
            {
                collection.BulkWrite(models);
            }

        }
        private class RatingStatisticsBson
        {
            public Object _id { get; set; }
            public string UserId { get; set; }
            public CountValues MunualStepsCount { get; set; }
            public CountValues TotalEarned { get; set; }
            public CountValues RootCounter { get; set; }
        }

    }

}
