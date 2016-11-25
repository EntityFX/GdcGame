using EntityFX.Gdcame.DataAccess.Repository.Contract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.UserRating;
using MongoDB.Bson;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class RatingRepository: IRatingRepository 
    {
        private IMongoDatabase Database
        {
            get; set;
        }
        public RatingRepository(IMongoDatabase database)
        {
            Database = database;
        }


        public RatingStatistics[] GetRaiting(int top = 500)
        {
            IMongoCollection<RatingStatistics> collection = Database.GetCollection<RatingStatistics>("RatingStatistics");
            var filter1 = new BsonDocument();
            RatingStatistics userTemp = new RatingStatistics
            {
                UserID = "Y39",
                MunualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
                TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
                RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            };
            collection.InsertOne(userTemp);
            var allUser = collection.Find(filter1).ToList();
            var uaser500 = allUser.OrderByDescending(a => a.TotalEarned.Total).Take(500).ToArray();
            return uaser500;
        }



        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            foreach (var userRating in ratingStatistics)
            {
                CreateOrReplaceUsersRatingStatistics(userRating);
            }
        }

        private void CreateOrReplaceUsersRatingStatistics(RatingStatistics ratingStatistics)
        {
            IMongoCollection<RatingStatistics> collection = Database.GetCollection<RatingStatistics>("RatingStatistics");
            var filter = Builders<RatingStatistics>.Filter.Eq("UserID", ratingStatistics.UserID);
            var findUser = collection.Find(filter).ToList();
            if (findUser.Count == 0)
            {
                collection.InsertOne(ratingStatistics);
            }
            else
            {
                var result = collection.ReplaceOne(filter, ratingStatistics);
            }

        }
    }
}
