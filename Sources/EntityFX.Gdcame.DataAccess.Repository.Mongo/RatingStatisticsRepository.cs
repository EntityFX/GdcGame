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

        public TopRatingStatistics GetRaiting(int top = 500)
        {
            IMongoCollection<RatingStatistics> ratingCollection = Database.GetCollection<RatingStatistics>("RatingStatistics");
            Func<string, string, TopStatisticsCounter[]> query = (string counter, string period) => ratingCollection
                .Aggregate()
                .Lookup("User", "_id", "_id", "UserInfo")
                .Sort(Builders<BsonDocument>.Sort.Descending(string.Format("{0}.{1}", counter, period)))
                .Project(_ =>
                    new TopStatisticsCounter()
                    {
                        UserId = (string)_["_id"],
                        Login = (string)(_["UserInfo.Login"][0]),
                        Value = (decimal)_[counter][period],
                    }
                )
                .Limit(500).ToList().ToArray();

            return new TopRatingStatistics
            {
                ManualStepsCount = new TopStatisticsAggregate()
                {
                    Day = query("ManualStepsCount", "Day"),
                    Week = query("ManualStepsCount", "Week"),
                    Total = query("ManualStepsCount", "Total"),
                },
                TotalEarned = new TopStatisticsAggregate()
                {
                    Day = query("TotalEarned", "Day"),
                    Week = query("TotalEarned", "Week"),
                    Total = query("TotalEarned", "Total"),
                },
                RootCounter = new TopStatisticsAggregate()
                {
                    Day = query("RootCounter", "Day"),
                    Week = query("RootCounter", "Week"),
                    Total = query("RootCounter", "Total"),
                },
            };
        }


        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
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
