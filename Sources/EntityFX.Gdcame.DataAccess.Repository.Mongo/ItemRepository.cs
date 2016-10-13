using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class ItemRepository :  IItemRepository
    {
        private IMongoDatabase Database
        {
            get; set;
        }

        public ItemRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public Item[] FindAll(GetAllFundsDriversCriterion criterion)
        {
            IMongoCollection<Item> itemCollection = Database.GetCollection<Item>("Item");

            var filter = new BsonDocument();
            if (itemCollection.Find<Item>(filter).Count() == 0)
            {
                Item[] items = new Item[] {
                    new Item() {
                    Id = 1, Name = "Item", Price = 300, IsUnlocked = true, InitialValue = 100
                        , Incrementors = new Dictionary<int, Incrementor>() {
                            { 0, new Incrementor() { Value = 1, IncrementorType = IncrementorTypeEnum.ValueIncrementor } },
                            { 1, new Incrementor() { Value = 10, IncrementorType = IncrementorTypeEnum.ValueIncrementor } },
                            { 2, new Incrementor() { Value = 0, IncrementorType = IncrementorTypeEnum.ValueIncrementor } }
                            }
                    }
                };
                itemCollection.InsertMany(items);
            }

            return itemCollection.Find(filter).SortBy(_ => _.Id).ToList().ToArray();
        }
    }
}
