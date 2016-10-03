using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.LocalStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Test.Entity
{
    [TestClass]
    public class MigrateTest
    {
        private MongoClient _mongoClient;


        class DictionaryRepresentationConvention : ConventionBase, IMemberMapConvention
        {
            private readonly DictionaryRepresentation _dictionaryRepresentation;
            public DictionaryRepresentationConvention(DictionaryRepresentation dictionaryRepresentation)
            {
                _dictionaryRepresentation = dictionaryRepresentation;
            }
            public void Apply(BsonMemberMap memberMap)
            {
                memberMap.SetSerializer(ConfigureSerializer(memberMap.GetSerializer()));
            }
            private IBsonSerializer ConfigureSerializer(IBsonSerializer serializer)
            {
                var dictionaryRepresentationConfigurable = serializer as IDictionaryRepresentationConfigurable;
                if (dictionaryRepresentationConfigurable != null)
                {
                    serializer = dictionaryRepresentationConfigurable.WithDictionaryRepresentation(_dictionaryRepresentation);
                }

                var childSerializerConfigurable = serializer as IChildSerializerConfigurable;
                return childSerializerConfigurable == null
                    ? serializer
                    : childSerializerConfigurable.WithChildSerializer(ConfigureSerializer(childSerializerConfigurable.ChildSerializer));
            }
        }

        public IMongoDatabase MongoDatabase
        {
            get
            {
                if (_mongoClient == null)
                    _mongoClient = new MongoClient("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame");
                return _mongoClient.GetDatabase(MongoUrl.Create("mongodb://admin:P%40ssw0rd@10.10.139.148:27017/gdcame?authSource=gdcame").DatabaseName);
            }
        }

        [TestMethod]
        public void TestMigrateLocalToMongo()
        {
            ConventionRegistry.Register(
"DictionaryRepresentationConvention",
new ConventionPack { new DictionaryRepresentationConvention(DictionaryRepresentation.ArrayOfArrays) },
_ => true);

            var itemsRepo = new EntityFX.Gdcame.DataAccess.Repository.LocalStorage.ItemRepository();
            var items = itemsRepo.FindAll(new DataAccess.Repository.Contract.Criterions.FundsDriver.GetAllFundsDriversCriterion());

            IMongoCollection<Item> itemCollection = MongoDatabase.GetCollection<Item>("Item");
            itemCollection.InsertMany(items);
            var mongoRepo = new EntityFX.Gdcame.DataAccess.Repository.Mongo.ItemRepository(MongoDatabase);
        }
    }
}
