db.dropDatabase();
db.createCollection("User");
db.User.createIndex({ Login: 1 }, { name: "LoginIndex", unique: true });

db.createCollection("Item");
db.createCollection("Counter");
db.createCollection("StoredGameDataWithUserId");