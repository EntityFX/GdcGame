namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.Common
{
    using System;

    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;

    using MongoDB.Bson;
    using MongoDB.Driver;

    class ServerRepository : IServerRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }

        public ServerRepository(IMongoDatabase database)
        {
            this.Database = database;
        }

        public Server[] FindServers()
        {
            IMongoCollection<Server> collection = this.Database.GetCollection<Server>("Server");
            var filter = new BsonDocument();
            return collection.Find<Server>(filter).ToList().ToArray();
        }

        public void Create(string server)
        {
            IMongoCollection<Server> collection = this.Database.GetCollection<Server>("Server");
            if (collection.Find(_ => _.Address == server).Any()) return;

            collection.InsertOne(new Server() { Address = server, CreateDateTime = DateTime.Now });
        }

        public void Delete(string server)
        {
            IMongoCollection<Server> users = this.Database.GetCollection<Server>("Server");
            var filter = Builders<Server>.Filter.Eq("Address", server);
            users.DeleteOne(filter);
        }
    }
}
