using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EntityFx.Gdcame.Test.Unit
{
    [TestClass]
    public class DataAccessTest
    {
        [TestMethod]
        public void Test()
        {
            var client = new MongoClient("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame");
            var database = client.GetDatabase("gdcame");
            RatingStatisticsRepository testRating = new RatingStatisticsRepository(database);
            RatingStatistics userTemp = new RatingStatistics
            {
                Id = 45,
                UserId = "Y391",
                MunualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
                TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
                RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            };
            RatingStatistics userTemp2 = new RatingStatistics
            {
                Id = 12,
                UserId = "Y392",
                MunualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
                TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
                RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            };
            RatingStatistics userTemp3 = new RatingStatistics
            {
                Id = 123,
                UserId = "Y393",
                MunualStepsCount = new CountValues { Day = 123, Week = 233, Total = 588 },
                TotalEarned = new CountValues { Day = 623, Week = 745, Total = 987 },
                RootCounter = new CountValues { Day = 623, Week = 745, Total = 987 }
            };
            RatingStatistics[] Rating = new RatingStatistics[] { userTemp, userTemp2, userTemp3 };
            testRating.CreateOrUpdateUsersRatingStatistics(Rating);

        }
    }
}
