using EntityFX.Gdcame.DataAccess.Repository.Contract;
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
using EntityFX.Gdcame.DataAccess.Contract.User;

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



        public RatingStatisticsUserInfo[] GetRaiting(int top = 500)
        {
            IMongoCollection<RatingStatistics> ratingCollection = Database.GetCollection<RatingStatistics>("RatingStatistics");
            var aggregate = ratingCollection.Aggregate()
                .Lookup("User", "_id", "_id", "UserInfo");
            var ratingStatistics = aggregate.Project(doc => new RatingStatisticsUserInfo()
            {
                UserId = (string)doc["_id"],
                Login = (string)(doc["UserInfo.Login"][0]),
                ManualStepsCount = new CountValues()
                {
                    Day = (decimal)doc["ManualStepsCount"]["Day"],
                    Week = (decimal)doc["ManualStepsCount"]["Week"],
                    Total = (decimal)doc["ManualStepsCount"]["Total"]
                },
                RootCounter = new CountValues()
                {
                    Day = (decimal)doc["RootCounter"]["Day"],
                    Week = (decimal)doc["RootCounter"]["Week"],
                    Total = (decimal)doc["RootCounter"]["Total"]
                },
                TotalEarned = new CountValues()
                {
                    Day = (decimal)doc["TotalEarned"]["Day"],
                    Week = (decimal)doc["TotalEarned"]["Week"],
                    Total = (decimal)doc["TotalEarned"]["Total"]
                },
            }).ToList().ToArray();
            //.Unwind("UserInfo")
            //,"Login":"UserInfo.Login","_id":0)
            //;
            //List<RatingStatisticsUserInfo> ratingStatistics = new List<RatingStatisticsUserInfo>();
            //foreach (var doc in aggregate)
            //{
            //    ratingStatistics.Add(new RatingStatisticsUserInfo()
            //    {
            //        UserId = (string)doc["_id"],
            //        Login = (string)doc["UserInfo"]["Login"],
            //        ManualStepsCount = new CountValues()
            //        {
            //            Day = (decimal)doc["ManualStepsCount"]["Day"],
            //            Week = (decimal)doc["ManualStepsCount"]["Week"],
            //            Total = (decimal)doc["ManualStepsCount"]["Total"]
            //        },
            //        RootCounter = new CountValues()
            //        {
            //            Day = (decimal)doc["ManualStepsCount"]["Day"],
            //            Week = (decimal)doc["ManualStepsCount"]["Week"],
            //            Total = (decimal)doc["ManualStepsCount"]["Total"]
            //        },
            //        TotalEarned = new CountValues()
            //        {
            //            Day = (decimal)doc["ManualStepsCount"]["Day"],
            //            Week = (decimal)doc["ManualStepsCount"]["Week"],
            //            Total = (decimal)doc["ManualStepsCount"]["Total"]
            //        },
            //    });
            //}
            return ratingStatistics.ToArray();
        }


        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            //foreach (var userRating in ratingStatistics)
            //{
            //    CreateOrReplaceUsersRatingStatistics(userRating);
            //}
            List<RatingStatistics> pushUsersRatingStatistics = new List<RatingStatistics>();
            foreach (var userRatingStatistic in ratingStatistics)
            {
                if (userRatingStatistic != null)
                {
                    pushUsersRatingStatistics.Add(userRatingStatistic);
                }
            }
            IMongoCollection<RatingStatistics> collection = Database.GetCollection<RatingStatistics>("RatingStatistics");

            var groupedByUserId = pushUsersRatingStatistics.GroupBy(_ => _.UserId).Select(_ => _.First()).ToArray();
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

    }

}
