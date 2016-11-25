using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{

    public class LocalRatingRepository : ILocalRatingRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }
        public LocalRatingRepository(IMongoDatabase database)
        {
            Database = database;
        }
        public void CleanOldHistory(TimeSpan period)
        {
            IMongoCollection<RatingHistory> collection = Database.GetCollection<RatingHistory>("RatingHistory");
            period = new TimeSpan(7, 0, 0, 0);
            var dataFilter = DateTime.Now.Subtract(period);
            var filter = Builders<RatingHistory>.Filter.Lt("Data", dataFilter);
            var result = collection.DeleteMany(filter);

        }
        public RatingStatistics[] GetRaiting(int top = 500)
        {
            //var filter = Builders<RatingStatistics>.Filter.Gte("TotalEarned.Total", 0);          
            //var user = collection.Find(filter);
            IMongoCollection<RatingStatistics> collection = Database.GetCollection<RatingStatistics>("RatingStatistics");
            var filter1 = new BsonDocument();           
            var allUser = collection.Find(filter1).ToList();
            var uaser500 = allUser.OrderByDescending(a => a.TotalEarned.Total).Take(500).ToArray();
            return uaser500;
        }

        public void PersistRatingHistory(RatingHistory ratingHistory)
        {

            IMongoCollection<RatingHistory> collection = Database.GetCollection<RatingHistory>("RatingHistory");
            RatingHistory userRatingHistory = new RatingHistory
            {
                UserID = "70",
                Data = new DateTime(2016, 11, 23),
                ManualStepsCount = 853,
                TotalEarned = 456,
                RootCounter = 156,

            };
            collection.InsertOneAsync(userRatingHistory);
           // collection.InsertOneAsync(ratingHistory);
        }

        public void ReadHistoryWithUsersIds(string[] userslds, TimeSpan period)
        {
            period = new TimeSpan(7, 0, 0, 0);
            // List<RatingHistory> usersRating = new List<RatingHistory>();
            IMongoCollection<RatingHistory> collection = Database.GetCollection<RatingHistory>("RatingHistory");
            //foreach (var userId in userslds)
            //{              
            //    var filter1 = Builders<RatingHistory>.Filter.Eq("UserID",userId);
            //    var dataFilter = DateTime.Now.Subtract(period);
            //    var filter2 = Builders<RatingHistory>.Filter.Gte("Data",dataFilter);
            //    var filterAnd = Builders<RatingHistory>.Filter.And(new List<FilterDefinition<RatingHistory>> { filter1, filter2 });
            //    //BsonDocument periodFilter=new BsonDocument("Data",)
            //    //BsonDocument filter = new BsonDocument("UserID", userId);
            //    var userRating = collection.Find(filterAnd).ToList();
            //    usersRating.Add(userRating.First());
            //}
            var filter1 = Builders<RatingHistory>.Filter.In("UserID", userslds);
            var dataFilter = DateTime.Now.Subtract(period);
            var filter2 = Builders<RatingHistory>.Filter.Gte("Data", dataFilter);
            var filterAnd = Builders<RatingHistory>.Filter.And(new List<FilterDefinition<RatingHistory>> { filter1, filter2 });
            var usersRating = collection.Find(filterAnd).ToList();
            // return usersRating.ToArray();
            //  BsonDocument filter=new BsonDocument("UserID",)
            throw new NotImplementedException();
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
