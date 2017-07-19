using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.Server;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    class ServerRepository : IServerRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }

        public ServerRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public Server[] FindServers()
        {
            IMongoCollection<Server> collection = Database.GetCollection<Server>("Server");
            var filter = new BsonDocument();
            return collection.Find<Server>(filter).ToList().ToArray();
        }

        public void Create(string server)
        {
            IMongoCollection<Server> collection = Database.GetCollection<Server>("Server");
            if (collection.Find(_ => _.Address == server).Any()) return;

            collection.InsertOne(new Server() { Address = server, CreateDateTime = DateTime.Now });
        }

        public void Delete(string server)
        {
            IMongoCollection<Server> users = Database.GetCollection<Server>("Server");
            var filter = Builders<Server>.Filter.Eq("Address", server);
            users.DeleteOne(filter);
        }
    }
}
