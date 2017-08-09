namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class UserRepository : IUserRepository
    {
        private IMongoDatabase Database
        {
            get;  set;
        }

        public UserRepository(IMongoDatabase database)
        {
            this.Database = database;
        }

        public int Create(User user)
        {
            this.Database.GetCollection<User>("User").InsertOne(user);
            return 1;
        }

        public void Delete(string id)
        {
            IMongoCollection<User> users = this.Database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Eq("Id", id);
            users.DeleteOne(filter);
        }

        //TODO: delete finalAllCriterion, it is not used
        public IEnumerable<User> FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            IMongoCollection<User> users = this.Database.GetCollection<User>("User");
            var filter = new BsonDocument();
            return users.Find<User>(filter).ToList();
        }

        public IEnumerable<User> FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion)
        {
            return null;
            //IMongoCollection<User> users = Database.GetCollection<User>("Users");
            //var filter = Builders<User>.Filter.Eq("Login", String.FormatfindByIdCriterion.SearchString);
            //var result = users.Find(
            //    Query.Matches("story", "<Regex for: moon or cow or Neil>")
            //    ).ToList();
            //return result.FirstOrDefault();
        }

        public int Count()
        {
            return (int)this.Database.GetCollection<User>("User").Count(new BsonDocument());
        }

        public User FindById(GetUserByIdCriterion findByIdCriterion)
        {
            IMongoCollection<User> users = this.Database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Eq("Id", findByIdCriterion.Id);
            var result = users.Find(filter).ToList();
            return result.FirstOrDefault();
        }

        public User FindByName(GetUserByNameCriterion findByNameCriterion)
        {
            IMongoCollection<User> users = this.Database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Eq("Login", findByNameCriterion.Name);
            var result = users.Find(filter).ToList();
            return result.FirstOrDefault();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> FindChunked(GetUsersByOffsetCriterion offsetCriterion)
        {
            IMongoCollection<User> users = this.Database.GetCollection<User>("User");
            var filter = new BsonDocument();
            return users.Find(filter).Skip(offsetCriterion.Offset).Limit(offsetCriterion.Size).ToList();
        }

        public IEnumerable<User> FindWithIds(GetUsersWithIdsCriterion usersWithIdsCriterion)
        {
            IMongoCollection<User> collection = this.Database.GetCollection<User>("User");
            var filter1 = Builders<User>.Filter.In("_id", usersWithIdsCriterion.UsersIds);
            return collection.Find(filter1).ToList();
        }
    }
}
