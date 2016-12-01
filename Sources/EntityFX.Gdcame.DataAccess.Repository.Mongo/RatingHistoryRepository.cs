using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class RatingHistoryRepository : IRatingHistoryRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }
        public RatingHistoryRepository(IMongoDatabase database)
        {
            Database = database;
        }

        /// <summary>
        /// Clean Old History less than period
        /// </summary>
        /// <param name="period"></param>
        public void CleanOldHistory(TimeSpan period)
        {
            IMongoCollection<RatingHistory> collection = Database.GetCollection<RatingHistory>("RatingHistory");
            var dataFilter = DateTime.Now.Subtract(period);
            var filter = Builders<RatingHistory>.Filter.Lt("Data", dataFilter);
            var result = collection.DeleteMany(filter);
        }



        public RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period)
        {
            IMongoCollection<RatingHistoryBase> collection = Database.GetCollection<RatingHistoryBase>("RatingHistory");
            var filter1 = Builders<RatingHistoryBase>.Filter.In("UserId", userslds);
            var dataFilter = DateTime.Now.Subtract(period);
            var filter2 = Builders<RatingHistoryBase>.Filter.Gte("Data", dataFilter);
            var filterAnd = Builders<RatingHistoryBase>.Filter.And(new List<FilterDefinition<RatingHistoryBase>> { filter1, filter2 });
            var usersRating = collection.Find(filterAnd).ToList();

            return ConverterRatingHistoryBaseAsRatingHistory(usersRating).ToArray();
        }

        private List<RatingHistory> ConverterRatingHistoryBaseAsRatingHistory(List<RatingHistoryBase> ratingHistoryBase)
        {
            List<RatingHistory> rating = new List<RatingHistory>();
            foreach (var ratingHistory in ratingHistoryBase)
            {
                rating.Add(new RatingHistory
                {
                    UserId=ratingHistory.UserId,
                    Data=ratingHistory.Data,
                    ManualStepsCount=ratingHistory.ManualStepsCount,
                    RootCounter=ratingHistory.RootCounter,
                    TotalEarned=ratingHistory.TotalEarned,
                });
            }
            return rating;
        }

        public void PersistUsersRatingHistory(RatingHistory[] usersRatingHistory)
        {
            List<RatingHistory> pushUsersHistory = new List<RatingHistory>();
            foreach (var userRatingHistory in usersRatingHistory)
            {
                if (userRatingHistory != null)
                {
                    pushUsersHistory.Add(userRatingHistory);
                }
            }
            IMongoCollection<RatingHistory> collection = Database.GetCollection<RatingHistory>("RatingHistory");       
            collection.InsertMany(pushUsersHistory);
        }

        private class RatingHistoryBase
        {
            public Object _id { get; set; }
            public string UserId { get; set; }
            public DateTime Data { get; set; }
            public int ManualStepsCount { get; set; }
            public decimal TotalEarned { get; set; }
            public int RootCounter { get; set; }
        }

    }
}
