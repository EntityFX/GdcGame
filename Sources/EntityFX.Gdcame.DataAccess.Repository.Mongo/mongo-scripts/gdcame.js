db.dropDatabase();
db.createCollection("User");
db.User.createIndex({ Login: 1 }, { name: "LoginIndex", unique: true });

db.createCollection("Item");
db.createCollection("Counter");
db.createCollection("StoredGameDataWithUserId");

db.RatingHistory.drop();
db.createCollection("RatingHistory");
var ratingHistoryCollection = db.getCollection("RatingHistory");

ratingHistoryCollection.createIndex({ "UserId": "hashed" });

db.RatingStatistics.drop();
db.createCollection("RatingStatistics");
var ratingStatisticsCollection = db.getCollection("RatingStatistics");

ratingStatisticsCollection.createIndex({ "ManualStepsCount.Day": -1 }, { "name": "ManualStepsCount.Day_-1" });
ratingStatisticsCollection.createIndex({ "ManualStepsCount.Total": -1 }, { "name": "ManualStepsCount.Total_-1" });
ratingStatisticsCollection.createIndex({ "ManualStepsCount.Week": -1 }, { "name": "ManualStepsCount.Week-1" });
ratingStatisticsCollection.createIndex({ "RootCounter.Day": -1 }, { "name": "RootCounter.Day_-1" });
ratingStatisticsCollection.createIndex({ "RootCounter.Total": -1 }, { "name": "RootCounter.Total_-1" });
ratingStatisticsCollection.createIndex({ "RootCounter.Week": -1 }, { "name": "RootCounter.Week-1" });
ratingStatisticsCollection.createIndex({ "TotalEarned.Day": -1 }, { "name": "TotalEarned.Day_-1" });
ratingStatisticsCollection.createIndex({ "TotalEarned.Total": -1 }, { "name": "TotalEarned.Total_-1" });
ratingStatisticsCollection.createIndex({ "TotalEarned.Week": -1 }, { "name": "TotalEarned.Week-1" });