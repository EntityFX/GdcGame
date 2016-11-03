using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;
using EntityFX.Gdcame.DataAccess.Contract.User;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{

    public class UserRepository : IUserRepository
    {
        private IMongoDatabase Database
        {
            get;  set;
        }

        public UserRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public int Create(DataAccess.Contract.User.User user)
        {
            Database.GetCollection<User>("User").InsertOne(user);
            return 1;
        }

        public void Delete(string id)
        {
            IMongoCollection<User> users = Database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Eq("Id", id);
            users.DeleteOne(filter);
        }

        public DataAccess.Contract.User.User[] FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            IMongoCollection<User> users = Database.GetCollection<User>("User");
            var filter = new BsonDocument();
            return users.Find<User>(filter).ToList().ToArray();
        }

        public DataAccess.Contract.User.User[] FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion)
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
            return (int)Database.GetCollection<User>("User").Count(new BsonDocument());
        }

        public DataAccess.Contract.User.User FindById(GetUserByIdCriterion findByIdCriterion)
        {
            IMongoCollection<User> users = Database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Eq("Id", findByIdCriterion.Id);
            var result = users.Find(filter).ToList();
            return result.FirstOrDefault();
        }

        public DataAccess.Contract.User.User FindByName(GetUserByNameCriterion findByNameCriterion)
        {
            IMongoCollection<User> users = Database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Eq("Login", findByNameCriterion.Name);
            var result = users.Find(filter).ToList();
            return result.FirstOrDefault();
        }

        public void Update(DataAccess.Contract.User.User user)
        {
            throw new NotImplementedException();
        }
    }
}
