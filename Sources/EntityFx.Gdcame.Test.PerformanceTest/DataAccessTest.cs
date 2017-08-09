using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityFX.Gdcame.DataAccess.Repository.Mongo;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EntityFx.Gdcame.Test.Unit
{
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.RatingHistory;
    using EntityFX.Gdcame.DataAccess.Repository.Mongo.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    [TestClass]
    public class DataAccessTest
    {
        [TestMethod]
        public void CreateOrUpdateUsersRatingStatistics()
        {
            var client = new MongoClient("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame");
            var database = client.GetDatabase("gdcame");
            LocalRatingStatisticsRepository testRating = new LocalRatingStatisticsRepository(database);
            RatingStatistics userTemp = new RatingStatistics
            {
                // Id = 45,
                UserId = "Y391",
                ManualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
                TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
                RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            };
            RatingStatistics userTemp2 = new RatingStatistics
            {
                // Id = 12,
                UserId = "Y392",
                ManualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
                TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
                RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            };
            RatingStatistics userTemp3 = new RatingStatistics
            {
                // Id = 123,
                UserId = "Y393",
                ManualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
                TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
                RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            };
            RatingStatistics[] Rating = new[] { userTemp, userTemp2, userTemp3 };
            testRating.CreateOrUpdateUsersRatingStatistics(Rating);

        }

        [TestMethod]
        public void  GetRaiting()
        {
            var client = new MongoClient("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame");
            var database = client.GetDatabase("gdcame");
            LocalRatingStatisticsRepository _testStatisticsRating = new LocalRatingStatisticsRepository(database);
            RatingHistoryRepository _testHistoryRating = new RatingHistoryRepository(database, new Logger(null));
            var receivedData= _testStatisticsRating.GetRaiting(500);
        }

        [TestMethod]
        public void PersistRatingHistory()
        {
            var client = new MongoClient("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame");
            var database = client.GetDatabase("gdcame");
            LocalRatingStatisticsRepository _testStatisticsRating = new LocalRatingStatisticsRepository(database);
            RatingHistoryRepository _testHistoryRating = new RatingHistoryRepository(database, new Logger(null));
            RatingHistory ratingHistory = new RatingHistory
                                              {
                                                  Data = new DateTime(2016, 11, 28, 12, 25, 25),
                                                  UserId = "accepted",
                                                  ManualStepsCount = 122,
                                                  RootCounter = 225,
                                                  TotalEarned = 928,
                                              };

            // _testHistoryRating.PersistRatingHistory(ratingHistory);
        }

        [TestMethod]
        public void  ReadHistoryWithUsersIds()
        {
            var client = new MongoClient("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame");
            var database = client.GetDatabase("gdcame");
            RatingHistoryRepository _testHistoryRating = new RatingHistoryRepository(database, new Logger(null));
            string[] userslds = new[] { "accepted" };
            TimeSpan period = new TimeSpan(24, 0, 0);
            var history= _testHistoryRating.ReadHistoryWithUsersIds(new GetUsersRatingHistoryCriterion() { UsersIds = userslds, Period = period});

        }

        [TestMethod]
        public void CleanOldHistory()
        {
            var client = new MongoClient("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame");
            var database = client.GetDatabase("gdcame");
            RatingHistoryRepository _testHistoryRating = new RatingHistoryRepository(database, new Logger(null));
            TimeSpan period = new TimeSpan(1, 0, 0);
            _testHistoryRating.CleanOldHistory(period);
        }
    }
}
